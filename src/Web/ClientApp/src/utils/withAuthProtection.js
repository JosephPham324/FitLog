import React, { useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { getUserRole } from '../utils/tokenOperations';

const withAuthProtection = (WrappedComponent, allowedRoles = []) => {
  const ProtectedComponent = (props) => {
    const { isAuthenticated, loading } = useContext(AuthContext);
    const userRoles = getUserRole(); // Add userRoles state
    const navigate = useNavigate();
    const [unauthorized, setUnauthorized] = useState(false);

    console.log('isAuthenticated in ProtectedComponent:', isAuthenticated);
    console.log('loading in ProtectedComponent:', loading);
    console.log('userRoles in ProtectedComponent:', userRoles);

    useEffect(() => {
      if (!loading) {
        
        if (!isAuthenticated) {
          navigate("/login");
        }
        if (allowedRoles != null && allowedRoles === []){
          return <WrappedComponent {...props} />;
        }
        if (allowedRoles.length > 0 && !allowedRoles.some(role => userRoles.includes(role))) {
          setUnauthorized(true);
          setTimeout(() => {
            navigate("/");
          }, 2000); // Redirect after 2 seconds
        }
      }
    }, [isAuthenticated, loading, userRoles, navigate]);

    if (loading) {
      return <div>Loading...</div>; // Show a loading indicator while checking authentication
    }

    if (unauthorized) {
      return <div>Unauthorized. Redirecting...</div>; // Show unauthorized message
    }

    return <WrappedComponent {...props} />;
  };

  return ProtectedComponent;
};

export default withAuthProtection;





//HOW TO USE

//Sample component to be protected
//const ProtectedComponent = () => {
//    return <div>Protected Content</div>;
//};


//Import component and withAuthProtection in the file
//import withAuthProtection from './path/to/withAuthProtection';
//import ProtectedComponent from './path/to/ProtectedComponent';

//Wrap the component with withAuthProtection
//const ProtectedComponentWithAuth = withAuthProtection(ProtectedComponent);

//Use the component like normal
//<ProtectedComponentWithAuth />
