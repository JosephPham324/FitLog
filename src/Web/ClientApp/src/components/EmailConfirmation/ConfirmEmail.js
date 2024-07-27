import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { Container, Alert } from 'reactstrap';

const ConfirmEmail = () => {
  const [status, setStatus] = useState('');
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const queryParams = new URLSearchParams(location.search);
    const token = queryParams.get('token');
    const email = queryParams.get('email');

    const confirmEmail = async () => {
      try {
        const response = await axios.put(`${process.env.REACT_APP_BACKEND_URL}/Users/confirm-email`, { token, email });
        if (response.data.success) {
          setStatus('Your email has been confirmed.');
          setTimeout(() => navigate('/login'), 3000); // Redirect to login after 3 seconds
        } else {
          setStatus('Email confirmation failed. Please try again.');
        }
      } catch (error) {
        setStatus('An error occurred while confirming your email.');
      }
    };

    if (token && email) {
      confirmEmail();
    } else {
      setStatus('Invalid email confirmation link.');
    }
  }, [location.search, navigate]);

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
      <Alert color={status.includes('failed') || status.includes('error') ? 'danger' : 'success'}>
        {status}
      </Alert>
    </Container>
  );
};

export default ConfirmEmail;
