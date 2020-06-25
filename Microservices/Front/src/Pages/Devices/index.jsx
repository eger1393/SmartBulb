import React, {useEffect, useState} from 'react'
import {ExpansionPanel, ExpansionPanelDetails, ExpansionPanelSummary, Typography} from "@material-ui/core";
import {ExpandMore, WbIncandescent} from '@material-ui/icons';
import {apiGetDeviceList, apiGetDeviceState, apiSetDeviceState} from "../../Api/device";
import {Container, DeviceListContainer, DeviceStateControlsContainer, PowerIcon} from "./styled";
import TextField from "../../Components/DebouncedNumberField";

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

    const handleCallTpLinkApi = async (deviceId, newState) => {
        let res = await apiSetDeviceState(deviceId, {...newState});
        handleChangeDeviceState(deviceId, res);
        //setDevices(prev => prev.map(x => x.deviceId === deviceId ? {...x, ...res} : {...x}))
    };

    const handleChangeDeviceState = (deviceId, mergedState) => {
        setDevices(prev => prev.map(x => x.deviceId === deviceId ? {...x, ...mergedState} : {...x}))
    };

    const getBulbColor = device => {
        if (device.on_off === 1)
            return `hsl(${device.hue}, ${device.saturation}%, 65%)`;
        return '';
    };

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
                        <WbIncandescent style={{color: getBulbColor(x)}}/>
                        <Typography style={{marginLeft: '20px', marginRight: 'auto'}}>{x.alias}</Typography>
                    </ExpansionPanelSummary>
                    <ExpansionPanelDetails>
                        <DeviceStateControlsContainer>
                            <div style={{display: 'flex', flexDirection: 'column'}}>
                                <TextField
                                    type="number"
                                    max={100}
                                    min={0}
                                    label="Яроксть"
                                    value={x.brightness}
                                    debounceCallback={() => handleCallTpLinkApi(x.deviceId, {...x})}
                                    debounce={500}
                                    onChange={e => handleChangeDeviceState(x.deviceId, {brightness: e.target.value})}
                                />
                                <TextField
                                    type="number"
                                    label="Оттенок"
                                    max={360}
                                    min={0}
                                    value={x.hue}
                                    debounceCallback={() => handleCallTpLinkApi(x.deviceId, {...x})}
                                    debounce={500}
                                    onChange={e => handleChangeDeviceState(x.deviceId, {hue: e.target.value})}
                                />
                                <TextField
                                    type="number"
                                    max={100}
                                    min={0}
                                    label="Насыщенность"
                                    value={x.saturation}
                                    debounceCallback={() => handleCallTpLinkApi(x.deviceId, {...x})}
                                    debounce={500}
                                    onChange={e => handleChangeDeviceState(x.deviceId, {saturation: e.target.value})}
                                />
                            </div>
                            <div>
                                <PowerIcon poweron={x.on_off}
                                           onClick={() => handleCallTpLinkApi(x.deviceId, {on_off: !x.on_off})}/>
                            </div>
                        </DeviceStateControlsContainer>
                    </ExpansionPanelDetails>
                </ExpansionPanel>
            )}

        </DeviceListContainer>
    </Container>)
};

export default DevicesPage