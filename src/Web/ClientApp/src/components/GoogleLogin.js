import React from 'react';
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import './login.css';
import axios from 'axios';
import { FaUser, FaLock } from 'react-icons/fa';
import { FcGoogle } from 'react-icons/fc';
import { FaFacebookF } from 'react-icons/fa';
import logo from '../assets/Logo.png';
import image7 from '../assets/image7.png';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';

const Login = () => {
  const handleGoogleLoginSuccess = async (credentialResponse) => {
    console.log('Google login successful:', credentialResponse);

    const token = credentialResponse.credential;

    console.log(`${process.env.REACT_APP_BACKEND_URL}/api/auth/google-login`);
    try {
      const response = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/Authentication/google-login`, {
        token,
      });
      console.log(response);

      const jwtToken = response.data; // Directly use response.data
      // Save the JWT token to local storage or a state management library
      localStorage.setItem('jwtToken', jwtToken);
      console.log('JWT Token:', jwtToken);
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
      const result = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/Authentication/facebook-login`, {
        name: name,
        email: email,
        UserId : id
      });
      const jwtToken = result.data;
      localStorage.setItem('jwtToken', jwtToken);
      console.log('JWT Token:', jwtToken);
    } catch (error) {
      console.error('Error sending token to backend:', error);
    }
  };

  const handleFacebookLoginFailure = (error) => {
    console.log('Facebook login failed:', error);
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

            <form className="login-form">
              <div className="form-group">
                <FaUser className="icon" />
                <input type="text" placeholder="Name" required />
              </div>

              <div className="form-group">
                <FaLock className="icon" />
                <input type="password" placeholder="Password" required />
              </div>

              <div className="form-options">
                <label className="checkbox-label">
                  <input type="checkbox" /> Remember me
                </label>
                <a href="#">Forgot Password?</a>
              </div>

              <button type="submit" className="btn" color="#007bff">
                <span className="gradient-text">LOGIN</span>
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
