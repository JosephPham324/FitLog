import React from 'react';
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import './login.css';
import { FaUser, FaLock } from 'react-icons/fa';
import { FcGoogle } from 'react-icons/fc';
import { FaFacebookF } from 'react-icons/fa';
import logo from '../assets/Logo.png';
import image7 from '../assets/image7.png';

const Login = () => {
  const handleGoogleLoginSuccess = (credentialResponse) => {
    console.log('Google login successful:', credentialResponse);
    // Handle the successful login response here
    // You can send the token to your backend for further processing
  };

  const handleGoogleLoginFailure = (error) => {
    console.log('Google login failed:', error);
    // Handle login failure here
  };

  return (
    <GoogleOAuthProvider clientId="YOUR_GOOGLE_CLIENT_ID">
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

              <button type="submit" className="btn">
                <span className="text-login">LOGIN</span>
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
                      <FcGoogle className="icon" /> Đăng nhập bằng Google
                    </button>
                  )}
                />

                <button type="button" className="btn facebook">
                  <FaFacebookF className="icon" /> Facebook
                </button>
              </div>

              <a href="#" className="signup-link">Sign Up</a>
            </form>

          </div>
        </div>
      </div>
    </GoogleOAuthProvider>
  );
};

export default Login;
