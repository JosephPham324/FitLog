import React from 'react';
import { Container, Alert } from 'reactstrap';

const RecoverConfirmation = () => {
  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
      <Alert color="success" className="text-center">
        <h2>Password Reset Successful</h2>
        <p>Your password has been reset. You can now log in with your new password.</p>
      </Alert>
    </Container>
  );
};

export default RecoverConfirmation;
