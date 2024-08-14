import React, { useState, useContext, useEffect } from 'react';
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import './login.css';
import axios from 'axios';
import { FaUser, FaLock } from 'react-icons/fa';
import { FcGoogle } from 'react-icons/fc';
import { FaFacebookF } from 'react-icons/fa';
import logo from '../assets/Logo.png';
import image7 from '../assets/image7.png';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';
import { setCookie } from '../utils/cookiesOperations';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';


const Login = () => {
  const navigate = useNavigate();
  const { isAuthenticated, login } = useContext(AuthContext);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/'); // Redirect to index page if already authenticated
    }
  }, [isAuthenticated, navigate]);

  const setAuthCookies = (jwtToken, rememberMe = false) => {
    const parts = jwtToken.split('.');
    const expDay = rememberMe

    setCookie('jwtHeaderPayload', `${parts[0]}.${parts[1]}`, 1);
    setCookie('jwtSignature', parts[2], 1);
  };

  const SignIn = (jwtToken) => {
    setAuthCookies(jwtToken);
    login();
    //redirect to index
  };

  const handleGoogleLoginSuccess = async (credentialResponse) => {
    console.log('Google login successful:', credentialResponse);

    const token = credentialResponse.credential;

    try {
      const response = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/Authentication/google-login`, { token });

      console.log(response);

      const responseData = response.data;
      if (responseData.result.success === true) {
        const token = responseData.token;
        SignIn(token);
      } else {
        setError(responseData.result.errors.join(' '))
      }
      //setAuthCookies(responseData);
      console.log('JWT Token:', responseData.token);
    } catch (error) {
      console.error('Error sending token to backend:', error);
    }
  };

  const handleGoogleLoginFailure = (error) => {
    console.log('Google login failed:', error);
  };

  const handleFacebookLoginSuccess = async (response) => {
    console.log('Facebook login successful:', response);
    const { name, email, id } = response;

    try {
      const response = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/Authentication/facebook-login`, { name, email, UserId: id });
      const responseData = response.data;
      if (responseData.result.success === true) {
        const token = responseData.token;
        SignIn(token);
      } else {
        setError(responseData.result.errors.join(' '))
      }
      console.log('JWT Token:', responseData.token);
    } catch (error) {
      console.error('Error sending token to backend:', error);
    }
  };

  const handleFacebookLoginFailure = (error) => {
    console.log('Facebook login failed:', error);
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const response = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/Authentication/password-login`, { username, password });
      //const loginResult = response.data;
      const responseData = response.data;
      if (responseData.result.success === true) {
        const token = responseData.token;
        SignIn(token);
      } else {
        setError(responseData.result.errors.join(' '))
      }
      //if (loginResult.success) {
      //  SignIn(loginResult.token);
      //  console.log('JWT Token:', loginResult.token);
      //} else {
      //  setError('Invalid username or password');
      //}
    } catch (error) {
      console.error('Error during login:', error);
      setError('An error occurred during login');
    }
  };

  return (
    <GoogleOAuthProvider clientId={process.env.REACT_APP_GOOGLE_CLIENT_ID}>
      <div className="container-login">
        <div className="left">
          <img src={image7} alt="Fitness" className="fitness" />
        </div>
        <div className="right">
          <div className="login-container">
            <img src={logo} alt="Logo" className="logo-login" />

            <form className="login-form" onSubmit={handleFormSubmit}>
              <div className="form-group">
                <FaUser className="icon" />
                <input
                  type="text"
                  placeholder="Name"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  required
                />
              </div>

              <div className="form-group">
                <FaLock className="icon" />
                <input
                  type="password"
                  placeholder="Password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </div>

              {error && <div className="error">{error}</div>}

              <div className="form-options">
                <label className="checkbox-label">
                  <input type="checkbox" /> Remember me
                </label>
                <a href="/recover-account/initiate">Forgot Password?</a> {/* Added recover password link */}
              </div>

              <button type="submit" className="btn-login">
                <p className="text-login">LOGIN</p>
              </button>

              <div className="social-login">
                <p>Or</p>

                <GoogleLogin
                  onSuccess={handleGoogleLoginSuccess}
                  onError={handleGoogleLoginFailure}
                  render={(renderProps) => (
                    <button
                      type="button"
                      className="btn google"
                      onClick={renderProps.onClick}
                      disabled={renderProps.disabled}
                    >
                      <FcGoogle className="icon" /> Google
                    </button>
                  )}
                />

                <FacebookLogin
                  appId={process.env.REACT_APP_FACEBOOK_APP_ID}
                  autoLoad={false}
                  fields="name,email"
                  callback={handleFacebookLoginSuccess}
                  onFailure={handleFacebookLoginFailure}
                  render={(renderProps) => (
                    <button
                      type="button"
                      className="btn facebook"
                      onClick={renderProps.onClick}
                    >
                      <FaFacebookF className="icon" /> Facebook
                    </button>
                  )}
                />
              </div>

              <a href="/register" className="signup-link">Sign Up</a>
            </form>
          </div>
        </div>
      </div>
    </GoogleOAuthProvider>
  );
};

export default Login;
