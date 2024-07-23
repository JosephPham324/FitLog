import React, { useState } from 'react';
import axios from 'axios';

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
    <div>
      <h2>Recover Password</h2>
      {message && <p>{message}</p>}
      {error && <p>{error}</p>}
      <form onSubmit={handleFormSubmit}>
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <button type="submit">Send Recovery Email</button>
      </form>
    </div>
  );
};

export default RecoverInitiate;
