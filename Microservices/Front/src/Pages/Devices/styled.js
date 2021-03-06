import styled from 'styled-components'
import {PowerSettingsNew} from '@material-ui/icons';

export const Container = styled.div`
    display: flex;
    justify-content: center;
`;

export const DeviceListContainer = styled.div`
    margin-top: 50px;
    width: 600px;
`;

export const PowerIcon = styled(PowerSettingsNew)`
    width: 24px;
    height: 24px;
    box-sizing: border-box;
    ${props => props.poweron === true ? `
        border: 2px solid #11b83d;
        border-radius: 50%;
    ` : ``}
    &:hover{
       cursor: pointer;
       opacity: 0.6;
    }
`;

export const DeviceStateControlsContainer = styled.div`
    display: flex;
    flex: 1;
    align-items: center;
    justify-content: space-between;
`;