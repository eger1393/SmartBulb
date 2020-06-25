import React, {useEffect, useState} from 'react'
import {ExpansionPanel, ExpansionPanelDetails, ExpansionPanelSummary, Typography} from "@material-ui/core";
import {ExpandMore, WbIncandescent} from '@material-ui/icons';
import {apiGetDeviceList, apiGetDeviceState} from "../../Api/device";
import {Container, DeviceListContainer, PowerIcon} from "./styled";

/*
device:
  alias: string, Название устройства заданное пользователем
  deviceId: string, Ид устройства
  deviceModel: string, Модель
  deviceName: string, Название устройства заданное производителем
  deviceType: string, Тип(нас интересует только "IOT.SMARTBULB")
  role: strung(number), Коль(пока хз что это, вроде как просто цифра
  status: string(number), Статус(0 - выкл, 1 - вкл)
 */

const DevicesPage = () => {
    const [devices, setDevices] = useState();
    const [isLoaded, setIsLoaded] = useState(false);
    const [expanded, setExpanded] = useState("");
    useEffect(() => {
        (async () => {
            let devices = await apiGetDeviceList();
            // Нас интересуют только лампы
            devices = devices.filter(x => x?.deviceType === 'IOT.SMARTBULB');
            let deviceWithState = [];
            for (let device of devices) {
                let res = await apiGetDeviceState(device.deviceId);
                deviceWithState.push({...device, ...res});
            }
            setDevices(deviceWithState);
            setIsLoaded(true);
        })();
    }, []);


    if (!isLoaded) return <>Loading...</>;
    console.log(devices);
    return (<Container>
        <DeviceListContainer>
            {devices.map(x =>
                <ExpansionPanel
                    key={x.deviceId}
                    expanded={expanded === x.deviceId}
                    onChange={() => setExpanded(expanded !== x.deviceId ? x.deviceId : '')}
                >
                    <ExpansionPanelSummary
                        expandIcon={<ExpandMore/>}
                    >
                        {/*TODO сделать нормальный расчет цвета, который учитывает яркость(сейчас есть косяки со 100% яркостью)*/}
                        <WbIncandescent style={{color: `hsl(${x.hue}, ${x.saturation}%, 65%)`}}/>
                        <Typography style={{marginLeft: '20px', marginRight: 'auto'}}>{x.alias}</Typography>
                    </ExpansionPanelSummary>
                    <ExpansionPanelDetails>
                        <Typography>
                            <PowerIcon poweron={x.on_off}/>
                        </Typography>
                    </ExpansionPanelDetails>
                </ExpansionPanel>
            )}

            <ExpansionPanel expanded={expanded === '1'} onChange={() => setExpanded(expanded !== '1' ? '1' : '')}>
                <ExpansionPanelSummary
                    expandIcon={<ExpandMore/>}
                >
                    <WbIncandescent/>
                    <Typography style={{marginLeft: '20px', marginRight: 'auto'}}>{'test2'}</Typography>
                    <PowerIcon poweron={0}/>
                </ExpansionPanelSummary>
                <ExpansionPanelDetails>
                    <Typography>
                        Nulla facilisi. Phasellus sollicitudin nulla et quam mattis feugiat. Aliquam eget
                        maximus est, id dignissim quam.
                    </Typography>
                </ExpansionPanelDetails>
            </ExpansionPanel>

            <ExpansionPanel expanded={expanded === '2'} onChange={() => setExpanded(expanded !== '2' ? '2' : '')}>
                <ExpansionPanelSummary
                    expandIcon={<ExpandMore/>}
                >
                    <WbIncandescent/>
                    <Typography style={{marginLeft: '20px', marginRight: 'auto'}}>{'test3'}</Typography>
                    <PowerIcon poweron={1}/>
                </ExpansionPanelSummary>
                <ExpansionPanelDetails>
                    <Typography>
                        Nulla facilisi. Phasellus sollicitudin nulla et quam mattis feugiat. Aliquam eget
                        maximus est, id dignissim quam.
                    </Typography>
                </ExpansionPanelDetails>
            </ExpansionPanel>
        </DeviceListContainer>
    </Container>)
};

export default DevicesPage