import React, { useState } from 'react';
import { TextField, Container, InputAdornment, IconButton, InputLabel, FormControl, FormHelperText, Input, Typography, Button } from '@mui/material';
import { Grid, FormControlLabel, Checkbox, Alert } from '@mui/material';
import { Email, Lock, Phone, AccountCircle } from '@mui/icons-material';
import { FcGoogle } from 'react-icons/fc';
import './register.css';
import logo from '../assets/Logo.png';
import image23 from '../assets/image23.png';
import { FaFacebookF } from 'react-icons/fa';
import axios from 'axios'; // Ensure axios is imported
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';

const Register = () => {
  const [values, setValues] = useState({
    password: "",
    showPassword: false,
    passwordCheck: "",
    showPasswordCheck: false
  });

  const handleClickShowPassword = () => {
    setValues({
      ...values,
      showPassword: !values.showPassword,
    });
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  const handlePasswordChange = (prop) => (event) => {
    setValues({
      ...values,
      [prop]: event.target.value,
    });
  };

  const handleClickShowPasswordCheck = () => {
    setValues({
      ...values,
      showPasswordCheck: !values.showPasswordCheck,
    });
  };

  const handleMouseDownPasswordCheck = (event) => {
    event.preventDefault();
  };

  const handlePasswordCheckChange = (prop) => (event) => {
    setValues({
      ...values,
      [prop]: event.target.value,
    });
  };

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

  const [successMessage, setSuccessMessage] = useState('');


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
    const phoneRegex = /^[0-9]{10}$/; // Ensures exactly 10 digits
    return phoneRegex.test(phoneNumber);
  };

  const validatePassword = (password) => {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!_\-/,]).{8,}$/;
    return passwordRegex.test(password);
  };

  const handleSubmit = async (e) => {
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
    if (!values.password) {
      newErrors.password = 'This is a mandatory question.';
      valid = false;
    } else if (!validatePassword(values.password)) {
      newErrors.password = 'Password must be 8 characters long and include an uppercase letter, a lowercase letter, a number, and a special character (@,!,-,_,/).';
      valid = false;
    }
    if (!values.passwordCheck) {
      newErrors.confirmPassword = 'This is a mandatory question.';
      valid = false;
    } else if (values.password !== values.passwordCheck) {
      newErrors.confirmPassword = 'Passwords do not match.';
      valid = false;
    }
    if (!formData.phoneNumber) {
      newErrors.phoneNumber = 'This is a mandatory question.';
      valid = false;
    } else if (!validatePhoneNumber(formData.phoneNumber)) {
      newErrors.phoneNumber = 'Phone number only allows entering numbers without spaces or letters or special characters. Phone number must be exactly 10 digits  ';
      valid = false;
    }
    if (!formData.termsAccepted) {
      newErrors.termsAccepted = 'You must agree to the terms.';
      valid = false;
    }

    formData.password = values.password;

    setErrors(newErrors);

    if (valid) {
      console.log('Form Data:', formData);

      try {
        const response = await axios.post('https://localhost:44447/api/Users/register', formData);
        console.log('Registration successful:', response.data);
        setSuccessMessage('Registration successful!');
      } catch (error) {
        console.error('Registration error:', error.response?.data || error.message);
        if (error.response && error.response.data && error.response.data.errors) {
          const apiErrors = error.response.data.errors;
          const updatedErrors = { ...newErrors };

          if (apiErrors.Email) {
            updatedErrors.email = apiErrors.Email[0];
          }
          if (apiErrors.UserName) {
            updatedErrors.userName = apiErrors.UserName[0];
          }
          setErrors(updatedErrors);
        } else {
          // Handle other errors (network issues, etc.)
        }
      }
    }
  };

  return (
    <Container className="register-container">
      <div className="register-content">
        <div className="register-left">
          <img src={image23} alt="Background" className="background-image" />
          <div className="overlay"></div>
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
              <span className="gradient-text" style={{ fontSize: '50px' }}> REGISTER</span>
            </div>
            <Typography variant="body2" className="typography-black">User Name (*)</Typography>
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

            <Typography variant="body2" className="typography-black">Email (*)</Typography>
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

            <Typography variant="body2" className="typography-black">Password (*)</Typography>
            <FormControl variant="outlined" fullWidth margin="normal" error={!!errors.password} sx={{ border: '1px solid gray', borderRadius: '4px', padding: '8px' }}>
              <Input
                id="password"
                type={values.showPassword ? 'text' : 'password'}
                value={values.password}
                onChange={handlePasswordChange('password')}
                startAdornment={
                  <InputAdornment position="start">
                    <Lock />
                  </InputAdornment>
                }
                endAdornment={
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={handleClickShowPassword}
                      onMouseDown={handleMouseDownPassword}
                    >
                      {values.showPassword ? <Visibility /> : <VisibilityOff />}
                    </IconButton>
                  </InputAdornment>
                }
              />
              {errors.password && <FormHelperText>{errors.password}</FormHelperText>}
            </FormControl>

            <Typography variant="body2" className="typography-black">Confirm Password (*)</Typography>
            <FormControl
              variant="outlined"
              fullWidth
              margin="normal"
              sx={{ border: '1px solid gray', borderRadius: '4px', padding: '8px' }}
              error={!!errors.confirmPassword}
            >
              <Input
                id="confirmPassword"
                type={values.showPasswordCheck ? 'text' : 'password'}
                value={values.passwordCheck}
                onChange={handlePasswordCheckChange('passwordCheck')}
                startAdornment={
                  <InputAdornment position="start">
                    <Lock />
                  </InputAdornment>
                }
                endAdornment={
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={handleClickShowPasswordCheck}
                      onMouseDown={handleMouseDownPasswordCheck}
                    >
                      {values.showPasswordCheck ? <Visibility /> : <VisibilityOff />}
                    </IconButton>
                  </InputAdornment>
                }
              />
              {errors.confirmPassword && <FormHelperText>{errors.confirmPassword}</FormHelperText>}
            </FormControl>

            <Typography variant="body2" className="typography-black">Phone Number (*)</Typography>
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
                  <Typography variant="body2" className="typography-black">
                    I agree to FittLog's <a href="#">Terms of Use</a> and <a href="#">Privacy Policy</a>. (*)
                  </Typography>
                }
              />
            </Grid>
            {successMessage && (
              <Alert severity="success">{successMessage}</Alert>
            )}
            {errors.termsAccepted && <Typography color="error">{errors.termsAccepted}</Typography>}

            <Button type="submit" variant="contained" color="primary" fullWidth className="btn" style={{ height: '50px', borderRadius: '10px' }}>REGISTER</Button>

          </form>
          <Typography variant="body1" className="or-text">Or</Typography>
          <a href="https://localhost:44447/login" className="signin-link button">Sign In</a>
        </div>
      </div>
    </Container>
  );
};

export default Register;
