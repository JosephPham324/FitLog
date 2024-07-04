import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { Button, Form, Input, List } from 'antd';
import { UserOutlined } from '@ant-design/icons';
//import './Chat.css';

const Chat = () => {
  const [connection, setConnection] = useState(null);
  const [chat, setChat] = useState([]);
  const [message, setMessage] = useState('');

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:44447/api/chathub', {
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
        })
        .catch(e => console.log('Connection failed: ', e));
    }
    console.log(connection);
  }, [connection]);

  const sendMessage = async () => {
    if (message && connection) {
      try {
        await connection.invoke('SendMessage', "Mock user", message);
        setMessage('');
      } catch (e) {
        console.error('Sending message failed: ', e);
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
    </div>
  );
};

export default Chat;
