import React from 'react';
import ReactDOM from 'react-dom';
import {history} from './Services/history'
import './index.css';
import * as serviceWorker from './serviceWorker';
import Pages from "./Pages";
import {configureAxios} from "./Services/configureAxios";
import {Router} from "react-router";

configureAxios(history);

ReactDOM.render(
    <React.StrictMode>
        <Router history={history}>
            <Pages/>
        </Router>
    </React.StrictMode>,
    document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
