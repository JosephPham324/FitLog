import axios from 'axios';
import { getCookie } from './cookiesOperations';

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: process.env.REACT_APP_BACKEND_URL,
  withCredentials: true, // This ensures that cookies are sent with requests
});

// Add a request interceptor to include the cookies
axiosInstance.interceptors.request.use(
  (config) => {
    //const jwtHeaderPayload = document.cookie.split('; ').find(row => row.startsWith('jwtHeaderPayload='));
    //const jwtSignature = document.cookie.split('; ').find(row => row.startsWith('jwtSignature='));
    const jwtHeaderPayload = getCookie('jwtHeaderPayload');
    const jwtSignature = getCookie('jwtSignature');

    console.log(`Bearer ${jwtHeaderPayload}.${jwtSignature}`);
    if (jwtHeaderPayload && jwtSignature) {
      config.headers.Authorization = `Bearer ${jwtHeaderPayload}.${jwtSignature}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
