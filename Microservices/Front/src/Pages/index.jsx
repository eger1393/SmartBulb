import React from 'react'
import {Route, Switch} from "react-router";
import LoginPage from "./Login";
import PrivateRoute from "../Components/PrivateRoute";
import DevicesPage from "./Devices";

const Pages = ({}) => {
    return (
        <Switch>
            <Route path="/login">
                <LoginPage/>
            </Route>
            <PrivateRoute path={["/", "/departments"]}>
                <DevicesPage/>
            </PrivateRoute>
        </Switch>)
};

export default Pages