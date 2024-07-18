import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { Button, Form, Input, List } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import { getCookie } from '../../utils/cookiesOperations';

const Chat = () => {
  const [connection, setConnection] = useState(null);
  const [chat, setChat] = useState([]);
  const [message, setMessage] = useState('');
  const [chatUrls, setChatUrls] = useState([]);
  const [chatMedia, setChatMedia] = useState([]);

  useEffect( () => {
    const jwtHeaderPayload = getCookie('jwtHeaderPayload');
    const jwtSignature = getCookie('jwtSignature');
    const token = jwtHeaderPayload && jwtSignature ? `${jwtHeaderPayload}.${jwtSignature}` : null;
    console.log(token);
    
    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:44447/api/chathub', {
        accessTokenFactory: () => token, // Include the token in the headers
        transport: HttpTransportType.ServerSentEvents // Use Server-Sent Events
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(result => {
          console.log('Connected!');
          connection.on('ReceiveMessage', (user, message) => {
            setChat(chat => [...chat, { user, message }]);
          });

          connection.on('LoadMessages', (chatLines) => {
            console.log(chatLines);
            chatLines.forEach(chatLine => {
              console.log(chatLine);
              setChat(prevChat => [...prevChat, { user: chatLine.createdByNavigation.userName, message: chatLine.chatLineText }]);
            });

          })
        })
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection]);

  const sendMessage = async () => {
    if (message && connection) {
      try {
        await connection.invoke('SendMessage', 1, message);
        setMessage('');
      } catch (e) {
        console.error('Sending message failed: ', e);
      }
    }
  };

  const loadChatLines = async (chatId) => {
    if (connection) {
      try {
        var results = await connection.invoke('GetChatLines', chatId);
        console.log(results);
        //setChat(chatLines);
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

  return (
    <div>
      <List
        itemLayout="horizontal"
        dataSource={chat}
        renderItem={item => (
          <List.Item>
            <List.Item.Meta
              avatar={<UserOutlined />}
              title={item.user}
              description={item.message}
            />
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
          <Button type="primary" htmlType="submit">Send</Button>
        </Form.Item>
      </Form>
      <Button onClick={() => loadChatLines(1)}>Load Chat Lines</Button>
      <Button onClick={() => loadChatUrls(1)}>Load Chat URLs</Button>
      <Button onClick={() => loadChatMedia(1)}>Load Chat Media</Button>
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
