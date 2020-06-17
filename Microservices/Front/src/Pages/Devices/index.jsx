import React from 'react'
import {history} from '../../Services/history'
import {Button} from "@material-ui/core";

const DevicesPage = () => {
    return (<>
        <Button
            onClick={() => {
                localStorage.removeItem('userToken');
                history.push("/")
            }}
        >Logout</Button>
    test
    </>)
};

export default DevicesPage