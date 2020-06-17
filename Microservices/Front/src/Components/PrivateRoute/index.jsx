import React from 'react';
import {Redirect, Route} from 'react-router-dom';

export const PrivateRoute = ({children, roles, ...rest}) => (
    <Route {...rest} render={props => {
        let storageItem = localStorage.getItem('userToken');
        if (!storageItem) {
            // not logged in so redirect to login page with the return url
            return <Redirect to={{pathname: '/login'}}/>
        }
        return children;
    }}/>
);

export default PrivateRoute
