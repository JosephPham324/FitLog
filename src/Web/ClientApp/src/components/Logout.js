// src/components/Logout.js
import React, { useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const Logout = () => {
  const navigate = useNavigate();
  const { logout } = useContext(AuthContext);

  useEffect(() => {
    logout();
    navigate('/login'); // Redirect to login page after logout
  }, [logout, navigate]);

  return (
    <div>
      Logging out...
    </div>
  );
};

export default Logout;
