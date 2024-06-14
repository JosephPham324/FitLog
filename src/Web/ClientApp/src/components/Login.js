import React from 'react';
import './login.css';
import { FaUser, FaLock } from 'react-icons/fa';
import { FcGoogle } from 'react-icons/fc';
import { FaFacebookF } from 'react-icons/fa';
import logo from '../assets/Logo.png';
import image7 from '../assets/image7.png';


const Login = () => {
  return (
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

              <button type="button" className="btn google">
                <FcGoogle className="icon" /> Google
              </button>
              <button type="button" className="btn facebook">
                <FaFacebookF className="icon" /> Facebook
              </button>
            </div>

            <a href="#" className="signup-link">Sign Up</a>
          </form>

        </div>
      </div>
    </div>
  );
};

export default Login;