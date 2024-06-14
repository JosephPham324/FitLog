import React, { useState } from 'react';
import { Container, TextField, Button, Typography, Grid, InputAdornment, FormControlLabel, Checkbox } from '@mui/material';
import { Email, Lock, Phone, AccountCircle } from '@mui/icons-material';
import { FcGoogle } from 'react-icons/fc';
import './register.css';
import logo from '../assets/Logo.png';
import image23 from '../assets/image23.png';
import { FaFacebookF } from 'react-icons/fa';

const Register = () => {
  const [formData, setFormData] = useState({
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    termsAccepted: false,
  });

  const [errors, setErrors] = useState({
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    termsAccepted: '',
  });

  const handleChange = (e) => {
    const { name, value, checked, type } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const validateEmail = (email) => {
    const emailRegex = /^[a-zA-Z0-9._%+-]{4,64}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email) && email.length <= 256 && !/^\s|\s$/.test(email) && !/\.\./.test(email) && !/^-|-$/.test(email.split('@')[0]);
  };

  const validatePhoneNumber = (phoneNumber) => {
    const phoneRegex = /^[0-9]{10,15}$/;
    return phoneRegex.test(phoneNumber);
  };

  const validatePassword = (password) => {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!_\-/,]).{8,}$/;
    return passwordRegex.test(password);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    let valid = true;
    const newErrors = {
      userName: '',
      email: '',
      password: '',
      confirmPassword: '',
      phoneNumber: '',
      termsAccepted: '',
    };

    if (!formData.userName) {
      newErrors.userName = 'This is a mandatory question.';
      valid = false;
    }
    if (!formData.email) {
      newErrors.email = 'This is a mandatory question.';
      valid = false;
    } else if (!validateEmail(formData.email)) {
      newErrors.email = 'Email must be entered in the correct format.';
      valid = false;
    }
    if (!formData.password) {
      newErrors.password = 'This is a mandatory question.';
      valid = false;
    } else if (!validatePassword(formData.password)) {
      newErrors.password = 'Password must be 8 characters long and include an uppercase letter, a lowercase letter, a number, and a special character (@,!,-,_,/).';
      valid = false;
    }
    if (!formData.confirmPassword) {
      newErrors.confirmPassword = 'This is a mandatory question.';
      valid = false;
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match.';
      valid = false;
    }
    if (!formData.phoneNumber) {
      newErrors.phoneNumber = 'This is a mandatory question.';
      valid = false;
    } else if (!validatePhoneNumber(formData.phoneNumber)) {
      newErrors.phoneNumber = 'Phone number only allows entering numbers without spaces or letters or special characters.';
      valid = false;
    }
    if (!formData.termsAccepted) {
      newErrors.termsAccepted = 'You must agree to the terms.';
      valid = false;
    }

    setErrors(newErrors);

    if (valid) {
      console.log('Form Data:', formData);
      // Submit form data
    }
  };

  return (
    <Container className="register-container">
      <div className="register-content">
        <div className="register-left">
          <img src={image23} alt="Background" className="background-image" />
          <div className="overlay"></div> {/* Thêm lớp phủ */}
          <div className="logo-res">
            <img src={logo} alt="Fitlog Logo" className="logo-image" />
          </div>
          <Typography variant="h6" className="heading">
            <span className="gradient-text">WHAT</span> <span className="red-text"> DO YOU NEED</span> <span className="gradient-text">TO PREPARE WHEN GOING TO</span>
            <span className="red-text">THE GYM</span> <span className="gradient-text"> ?</span>
          </Typography>
        </div>
        <div className="register-right">
          <form className="register-form" onSubmit={handleSubmit}>
            <div className="register-text">
              <span className="gradient-text"> REGISTER</span>
            </div>
            <Typography variant="body2">User Name (*)</Typography>
            <TextField
              name="userName"
              variant="outlined"
              fullWidth
              margin="normal"
              value={formData.userName}
              onChange={handleChange}
              error={!!errors.userName}
              helperText={errors.userName}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <AccountCircle />
                  </InputAdornment>
                ),
              }}
            />

            <Typography variant="body2">Email (*)</Typography>
            <TextField
              name="email"
              variant="outlined"
              fullWidth
              margin="normal"
              value={formData.email}
              onChange={handleChange}
              error={!!errors.email}
              helperText={errors.email}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Email />
                  </InputAdornment>
                ),
              }}
            />

            <Typography variant="body2">Password (*)</Typography>
            <TextField
              name="password"
              type="password"
              variant="outlined"
              fullWidth
              margin="normal"
              value={formData.password}
              onChange={handleChange}
              error={!!errors.password}
              helperText={errors.password}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Lock />
                  </InputAdornment>
                ),
              }}
            />

            <Typography variant="body2">Confirm Password (*)</Typography>
            <TextField
              name="confirmPassword"
              type="password"
              variant="outlined"
              fullWidth
              margin="normal"
              value={formData.confirmPassword}
              onChange={handleChange}
              error={!!errors.confirmPassword}
              helperText={errors.confirmPassword}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Lock />
                  </InputAdornment>
                ),
              }}
            />

            <Typography variant="body2">Phone Number (*)</Typography>
            <TextField
              name="phoneNumber"
              variant="outlined"
              fullWidth
              margin="normal"
              value={formData.phoneNumber}
              onChange={handleChange}
              error={!!errors.phoneNumber}
              helperText={errors.phoneNumber}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Phone />
                  </InputAdornment>
                ),
              }}
            />

            <Grid container justifyContent="center">
              <FormControlLabel
                control={<Checkbox name="termsAccepted" checked={formData.termsAccepted} onChange={handleChange} />}
                label={
                  <Typography variant="body2">
                    I agree to FittLog's <a href="#">Terms of Use</a> and <a href="#">Privacy Policy</a>. (*)
                  </Typography>
                }
              />
            </Grid>
            {errors.termsAccepted && <Typography color="error">{errors.termsAccepted}</Typography>}

            <Button type="submit" variant="contained" color="primary" fullWidth className="btn">
              REGISTER
            </Button>
          </form>
          <Typography variant="body1" className="or-text">Or</Typography>
          <Grid container spacing={2} justify="center">
            <Grid item>
              <Button type="button" variant="contained" className="btn google">
                <FcGoogle className="icon" /> Google
              </Button>
            </Grid>
            <Grid item>
              <Button type="button" variant="contained" className="btn facebook">
                <FaFacebookF className="icon" /> Facebook
              </Button>
            </Grid>

          </Grid>
          <a href="#" className="signin-link">Sign In</a>
        </div>
      </div>
    </Container>
  );
};

export default Register;