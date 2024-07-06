import React, { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';

const withAuthProtection = (WrappedComponent) => {
    return (props) => {
        const { isAuthenticated } = useContext(AuthContext);

        if (!isAuthenticated) {
            return <div>You need to login to access this content.</div>;
        }

        return <WrappedComponent {...props} />;
    };
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
