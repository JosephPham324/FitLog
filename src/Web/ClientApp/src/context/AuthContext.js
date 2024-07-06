// src/context/AuthContext.js
import React, { createContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCookie, eraseCookie } from '../utils/cookiesOperations';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const jwtHeaderPayload = getCookie('jwtHeaderPayload');
    const jwtSignature = getCookie('jwtSignature');

    if (jwtHeaderPayload && jwtSignature) {
      setIsAuthenticated(true);
    }
  }, []);

  const login = () => {
    setIsAuthenticated(true);
    navigate('/'); // Redirect to index page after login
  };

  const logout = () => {
    setIsAuthenticated(false);
    eraseCookie('jwtHeaderPayload');
    eraseCookie('jwtSignature');
    navigate('/google-login'); // Redirect to login page after logout
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
