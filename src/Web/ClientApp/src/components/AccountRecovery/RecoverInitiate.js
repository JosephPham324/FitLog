import React, { useState } from 'react';
import axios from 'axios';
import { Container, Form, FormGroup, Label, Input, Button, Alert } from 'reactstrap';

const RecoverInitiate = () => {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setMessage('');
    setError('');

    try {
      const response = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/Users/recover-account`, { email });
      if (response.data.success) {
        setMessage('A recovery email has been sent. Please check your inbox.');
      } else {
        setError('An error occurred while initiating recovery.');
      }
    } catch (error) {
      console.error('Error during recovery initiation:', error);
      setError('An error occurred while initiating recovery.');
    }
  };

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
      <Form onSubmit={handleFormSubmit} className="p-4 border rounded shadow" style={{ width: '300px', backgroundColor: 'white' }}>
        <h2 className="text-center mb-4">Recover Password</h2>
        {message && <Alert color="success">{message}</Alert>}
        {error && <Alert color="danger">{error}</Alert>}
        <FormGroup>
          <Label for="email">Email:</Label>
          <Input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </FormGroup>
        <Button color="primary" type="submit" block>Send Recovery Email</Button>
      </Form>
    </Container>
  );
};

export default RecoverInitiate;
