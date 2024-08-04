import React, { createContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCookie, eraseCookie } from '../utils/cookiesOperations';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtHeaderPayload = getCookie('jwtHeaderPayload');
    const jwtSignature = getCookie('jwtSignature');
    console.log('Cookies:', jwtHeaderPayload, jwtSignature);

    if (jwtHeaderPayload && jwtSignature) {
      console.log('Setting isAuthenticated to true');
      setIsAuthenticated(true);
    } else {
      console.log('No valid cookies found, setting isAuthenticated to false');
      setIsAuthenticated(false);
    }
    setLoading(false); // Authentication check is complete
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
    <AuthContext.Provider value={{ isAuthenticated, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
