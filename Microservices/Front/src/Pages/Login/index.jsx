import React, {useState} from 'react';
import {useHistory} from 'react-router-dom'


import {ErrorContainer, FieldsContainer, ImageStyled, LoginContainer} from './styled';
import logo from '../../Images/devLogo.jpg'
import {Button, TextField} from "@material-ui/core";
import {apiAuthorize} from "../../Api/authorize";

const LoginPage = () => {
    const [fields, setFields] = useState({login: '', password: ''});
    const [error, setError] = useState('');
    const history = useHistory();
    const handleChange = (e) => {
        const {id, value} = e.target;
        setFields(prev => ({...prev, [id]: value}));
        setError('')
    };

    const handleLogin = async () => {
        if (!fields.login || !fields.password) {
            setError("Заполните все поля!");
            return;
        }
        try {
            let token = await apiAuthorize(fields.login, fields.password);
            localStorage.setItem('userToken', token);
            history.push('/');
        } catch (ex) {
            console.log(ex);
            setError("Не правильное сочетание логина/пароля")
        }
    };

    return (
        <LoginContainer>
            <FieldsContainer>
                <h2>Система управления умными лампами</h2>
                <TextField
                    id="login"
                    value={fields.login}
                    onChange={handleChange}
                    placeholder="Логин"
                />
                <TextField
                    id="password"
                    type="password"
                    value={fields.password}
                    onChange={handleChange}
                    placeholder="Пароль"
                />
                {error && <ErrorContainer>{error}</ErrorContainer>}
                <Button
                    onClick={handleLogin}
                    variant="outlined"
                    size="small"
                >Вход</Button>
            </FieldsContainer>
            <ImageStyled src={logo}/>
        </LoginContainer>
    );
}

export default LoginPage;