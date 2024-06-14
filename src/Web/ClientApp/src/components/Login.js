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

              <label className="terms-policy">
                <input type="checkbox" required /> I agree to FittLog's <a href="#">Terms of Use</a> and <a href="#">Privacy Policy</a>.
              </label>

            <button type="submit" className="btn">Login</button>
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

//import React from 'react';
//import './Login.css';
//import { FaUser, FaLock } from 'react-icons/fa';
//import { FcGoogle } from 'react-icons/fc';
//import { FaFacebookF } from 'react-icons/fa';
//import logoImage from '../asset/Logo.png';
//import fitnessImage from '../asset/image 7.png';
//import { useNavigate } from 'react-router-dom';

//const Login = () => {
//  const navigate = useNavigate();

//  return (
//    <div className="container-login">
//      <div className="left">
//        <img src={fitnessImage} alt="Fitness" className="fitness" />
//      </div>
//      <div className="right">
//        <div className="login-container">
//          <img src={logoImage} alt="Logo" className="logo" />
//          <form className="login-form">
//            <div className="form-group">
//              <FaUser className="icon" />
//              <input type="text" placeholder="Name" required />
//            </div>
//            <div className="form-group">
//              <FaLock className="icon" />
//              <input type="password" placeholder="Password" required />
//            </div>
//            <div className="form-options">
//              <label className="checkbox-label">
//                <input type="checkbox" /> Remember me
//              </label>
//              <a href="#">Forgot Password?</a>
//            </div>

//            <label className="terms-policy">
//              <input type="checkbox" required /> I agree to FittLog's <a href="#">Terms of Use</a> and <a href="#">Privacy Policy</a>.
//            </label>

//            <button type="submit" className="btn">Login</button>
//            <div className="social-login">
//              <p>Or</p>
//              <button type="button" className="btn google">
//                <FcGoogle className="icon" /> Google
//              </button>
//              <button type="button" className="btn facebook">
//                <FaFacebookF className="icon" /> Facebook
//              </button>
//            </div>
//            <a href="#" className="signup-link" onClick={() => navigate('/register')}>Sign Up</a>
//          </form>
//        </div>
//      </div>
//    </div>
//  );
//};

//export default Login;
