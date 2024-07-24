import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';
import { Container, Form, FormGroup, Label, Input, Button, Alert } from 'reactstrap';

const RecoverAccount = () => {
  const [token, setToken] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const queryParams = new URLSearchParams(location.search);
    const decodedToken = queryParams.get('token') ? decodeURIComponent(queryParams.get('token').replace(/ /g, '+')) : '';
    const decodedEmail = queryParams.get('email') ? decodeURIComponent(queryParams.get('email')) : '';
    setToken(decodedToken);
    setEmail(decodedEmail);
  }, [location.search]);
  console.log(token, email);
  const handleSubmit = async (e) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setError('Passwords do not match');
      return;
    }

    try {
      const response = await axios.put(`${process.env.REACT_APP_BACKEND_URL}/Users/reset-password`, { token, email, password });
      if (response.data.success) {
        navigate('/recover-account/confirmation');
      } else {
        setError(response.data.errors.join(', '));
      }
    } catch (err) {
      setError('An error occurred while resetting your password.');
    }
  };

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
      <Form onSubmit={handleSubmit} className="p-4 border rounded shadow" style={{ width: '300px', backgroundColor: 'white' }}>
        <h2 className="text-center mb-4">Reset Password</h2>
        {error && <Alert color="danger">{error}</Alert>}
        <FormGroup>
          <Label for="password">New Password:</Label>
          <Input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </FormGroup>
        <FormGroup>
          <Label for="confirmPassword">Confirm Password:</Label>
          <Input
            type="password"
            id="confirmPassword"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
        </FormGroup>
        <Button color="primary" type="submit" block>Reset Password</Button>
      </Form>
    </Container>
  );
};

export default RecoverAccount;
