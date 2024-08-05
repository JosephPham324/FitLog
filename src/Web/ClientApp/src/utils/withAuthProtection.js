import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const withAuthProtection = (WrappedComponent, allowedRoles = []) => {
  const ProtectedComponent = (props) => {
    const { isAuthenticated, loading, userRoles } = useContext(AuthContext);
    console.log('isAuthenticated in ProtectedComponent:', isAuthenticated);
    console.log('loading in ProtectedComponent:', loading);
    console.log('userRoles in ProtectedComponent:', userRoles);

    if (loading) {
      return <div>Loading...</div>; // Show a loading indicator while checking authentication
    }

    if (!isAuthenticated) {
      return <Navigate to="/login" />;
    }

    if (allowedRoles.length > 0 && !allowedRoles.some(role => userRoles.includes(role))) {
      return <Navigate to="/unauthorized" />; // Navigate to unauthorized page if user lacks required roles
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
