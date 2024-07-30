import React, { useState } from 'react';
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import './login.css';
import { FaUser, FaLock, FaEye, FaEyeSlash } from 'react-icons/fa';
import { FcGoogle } from 'react-icons/fc';
import { FaFacebookF } from 'react-icons/fa';
import logo from '../assets/Logo.png';
import image7 from '../assets/image7.png';

const Login = () => {
  const [passwordVisible, setPasswordVisible] = useState(false);

  const handleGoogleLoginSuccess = (credentialResponse) => {
    console.log('Google login successful:', credentialResponse);
    // Handle the successful login response here
    // You can send the token to your backend for further processing
  };

  const handleGoogleLoginFailure = (error) => {
    console.log('Google login failed:', error);
    // Handle login failure here
  };

  const togglePasswordVisibility = (event) => {
    event.preventDefault();
    setPasswordVisible(!passwordVisible);
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
                <input type="text" placeholder="Username" required />
              </div>

              <div className="form-group">
                <FaLock className="icon" />
                <div className="input-group">
                  <input
                    className="form-control"
                    type={passwordVisible ? 'text' : 'password'}
                    placeholder="Password"
                    required
                  />
                  <div className="input-group-addon">
                    <a href="#" onClick={togglePasswordVisibility}>
                      {passwordVisible ? <FaEyeSlash /> : <FaEye />}
                    </a>
                  </div>
                </div>
              </div>

              <div className="error">Invalid username or password</div>

              <div className="form-options">
                <label className="checkbox-label">
                  <input type="checkbox" /> Remember me
                </label>
                <a href="#">Forgot Password?</a>
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
                      <FcGoogle className="icon" /> Đăng nhập bằng Google
                    </button>
                  )}
                />

                <button type="button" className="btn facebook">
                  <FaFacebookF className="icon" /> Facebook
                </button>
              </div>
              <a href="https://localhost:44447/rigister" className="signup-link">Sign Up</a>
            </form>

          </div>
        </div>
      </div>
    </GoogleOAuthProvider>
  );
};

export default Login;
