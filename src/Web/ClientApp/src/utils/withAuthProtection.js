import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const withAuthProtection = (WrappedComponent) => {
  const ProtectedComponent = (props) => {
    const { isAuthenticated, loading } = useContext(AuthContext);
    console.log('isAuthenticated in ProtectedComponent:', isAuthenticated);
    console.log('loading in ProtectedComponent:', loading);

    if (loading) {
      return <div>Loading...</div>; // Show a loading indicator while checking authentication
    }

    if (!isAuthenticated) {
      return <Navigate to="/login" />;
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
