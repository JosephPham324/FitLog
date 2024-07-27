import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder, HttpTransportType, LogLevel } from '@microsoft/signalr';
import { Button, Form, Input, List } from 'antd';
import { UserOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { getCookie } from '../../utils/cookiesOperations';
import './Chat.css'; // Ensure to import your CSS file

const Chat = () => {
  const [connection, setConnection] = useState(null);
  const [chat, setChat] = useState([]);
  const [message, setMessage] = useState('');
  const [chatUrls, setChatUrls] = useState([]);
  const [chatMedia, setChatMedia] = useState([]);
  const [editingMessageId, setEditingMessageId] = useState(null);

  const chatId = 2; // Example chatId, replace with actual chatId as needed

  useEffect(() => {
    const jwtHeaderPayload = getCookie('jwtHeaderPayload');
    const jwtSignature = getCookie('jwtSignature');
    const token = jwtHeaderPayload && jwtSignature ? `${jwtHeaderPayload}.${jwtSignature}` : null;
    console.log(token);

    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:44447/api/chathub', {
        accessTokenFactory: () => token, // Include the token in the headers
        transport: HttpTransportType.LongPolling // Use Server-Sent Events transport

      })
      .withAutomaticReconnect([500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 10000])
      .configureLogging(LogLevel.Debug).build();

    //newConnection.serverTimeoutInMilliseconds = 1000 * 60 * 60; // 1 hour

    newConnection.onclose(() => {
      console.log('Connection closed. Reconnecting in 5 seconds...');
      setTimeout(() => {
        newConnection.start().then(() => {
          console.log('Reconnected!');
          newConnection.invoke('JoinChatGroup', chatId);
        }).catch(err => console.log('Reconnection failed: ', err));
      }, 1);
    });
    setConnection(newConnection);

  }, []);

  useEffect(() => {
    
    if (connection) {
      connection.start()
        .then(result => {
          console.log('Connected!');
          connection.invoke('JoinChatGroup', chatId);

          connection.on('ReceiveMessage', (chatId, user, message) => {
            setChat(chat => [...chat, { chatId, user, message }]);
          });

          connection.on('LoadMessages', (chatLines) => {
            console.log(chatLines);
            setChat([]);
            chatLines.forEach(chatLine => {
              console.log(chatLine);
              setChat(prevChat => [...prevChat, { id: chatLine.id, user: chatLine.createdByNavigation.userName, message: chatLine.chatLineText }]);
            });
          });

          connection.on('UpdatedMessage', (updatedLine) => {
            setChat(prevChat => prevChat.map(chatLine => chatLine.id === updatedLine.id ? updatedLine : chatLine));
          });

          connection.on('DeletedMessage', (deletedLine) => {
            setChat(prevChat => prevChat.filter(chatLine => chatLine.id !== deletedLine.id));
          });
        })
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection]);

  const sendMessage = async () => {
    if (message && connection) {
      try {
        if (editingMessageId) {
          await connection.invoke('UpdateChatLine', editingMessageId, message);
          setEditingMessageId(null);
        } else {
          await connection.invoke('SendMessage', chatId, message);
        }
        setMessage('');
      } catch (e) {
        console.error('Sending message failed: ', e);
      }
    }
  };

  const loadChatLines = async (chatId) => {
    if (connection) {
      try {
        await connection.invoke('GetChatLines', chatId);
      } catch (e) {
        console.error('Fetching chat lines failed: ', e);
      }
    }
  };

  const loadChatUrls = async (chatId) => {
    if (connection) {
      try {
        const urls = await connection.invoke('GetChatUrls', chatId);
        setChatUrls(urls);
      } catch (e) {
        console.error('Fetching chat URLs failed: ', e);
      }
    }
  };

  const loadChatMedia = async (chatId) => {
    if (connection) {
      try {
        const media = await connection.invoke('GetChatMedia', chatId);
        setChatMedia(media);
      } catch (e) {
        console.error('Fetching chat media failed: ', e);
      }
    }
  };

  const editMessage = (id, currentMessage) => {
    setEditingMessageId(id);
    setMessage(currentMessage);
  };

  const deleteMessage = async (id) => {
    if (connection) {
      try {
        await connection.invoke('DeleteChatLine', id);
      } catch (e) {
        console.error('Deleting message failed: ', e);
      }
    }
  };

  return (
    <div>
      <List
        itemLayout="horizontal"
        dataSource={chat}
        renderItem={item => (
          <List.Item className={item.user === 'Me' ? 'right-align' : ''}>
            <List.Item.Meta
              avatar={<UserOutlined />}
              title={item.user}
              description={item.message}
            />
            <div>
              <Button icon={<EditOutlined />} onClick={() => editMessage(item.id, item.message)} />
              <Button icon={<DeleteOutlined />} onClick={() => deleteMessage(item.id)} />
            </div>
          </List.Item>
        )}
      />
      <Form onFinish={sendMessage}>
        <Form.Item>
          <Input
            placeholder="Type a message..."
            value={message}
            onChange={e => setMessage(e.target.value)}
          />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">{editingMessageId ? 'Update' : 'Send'}</Button>
        </Form.Item>
      </Form>
      <Button onClick={() => loadChatLines(chatId)}>Load Chat Lines</Button>
      <Button onClick={() => loadChatUrls(chatId)}>Load Chat URLs</Button>
      <Button onClick={() => loadChatMedia(chatId)}>Load Chat Media</Button>
      <div>
        <h3>Chat URLs</h3>
        <ul>
          {chatUrls.map((url, index) => (
            <li key={index}><a href={url} target="_blank" rel="noopener noreferrer">{url}</a></li>
          ))}
        </ul>
      </div>
      <div>
        <h3>Chat Media</h3>
        <ul>
          {chatMedia.map((media, index) => (
            <li key={index}><a href={media} target="_blank" rel="noopener noreferrer">{media}</a></li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default Chat;
