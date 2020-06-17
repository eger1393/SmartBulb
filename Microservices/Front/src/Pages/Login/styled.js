import styled from 'styled-components';

export const LoginContainer = styled.div`
    display: flex;
    flex-direction: row;
    min-width: 100vw;
    min-height: 100vh;
`;

export const FieldsContainer = styled.div`
    width: 320px;
    display: flex;
    flex-direction: column;
    padding: 0 50px;
    padding-top: 100px;

    button {
        width: 150px;
        margin-left: auto;
        margin-top: 20px;
    }
`;

export const ImageStyled = styled.img`
    height: 100vh;
    width: calc(100vw - 320px);
    margin: 0 auto;
    object-fit: ${window.location.hostname === 'localhost' || window.location.hostname === 'dev.dspr.i-teco.ru' ?
    'contain' : 'fill'};
`;

export const ErrorContainer = styled.div`
    color: red;
    text-align: center;
`;