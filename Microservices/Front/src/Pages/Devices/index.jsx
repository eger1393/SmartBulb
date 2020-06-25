import React, {useEffect, useState} from 'react'
import {ExpansionPanel, ExpansionPanelDetails, ExpansionPanelSummary, Typography} from "@material-ui/core";
import {ExpandMore, WbIncandescent} from '@material-ui/icons';
import {apiGetDeviceList, apiGetDeviceState, apiSetDeviceState} from "../../Api/device";
import {Container, DeviceListContainer, DeviceStateControlsContainer, PowerIcon} from "./styled";
import {PhotoshopPicker} from 'react-color'
import {hsv} from 'color-convert'

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
                deviceWithState.push({
                    ...device,
                    color: {
                        h: res.hue,
                        s: res.saturation/100,
                        v: res.brightness/100,
                        a: 1,
                    },
                    power: !!res.on_off,
                });
            }
            setDevices(deviceWithState);
            setIsLoaded(true);
        })();
    }, []);

    const handleTogglePower = (device) => async () => {
        changeDeviceState(device.deviceId, {power: !device.power});
        await apiSetDeviceState(device.deviceId, {on_off: !device.power});
    };

    const handleSetColor = device => async color => {
        apiSetDeviceState(device.deviceId, {
            hue: Number.parseInt(color.hsv.h),
            saturation: Number.parseInt(color.hsv.s * 100),
            brightness: Number.parseInt(color.hsv.v * 100),
        });
        changeDeviceState(device.deviceId, {color: color.hsv})
    };

    const changeDeviceState = (deviceId, mergedState) => {
        setDevices(prev => prev.map(x => x.deviceId === deviceId ? {...x, ...mergedState} : {...x}))
    };

    const getBulbColor = device => {
        if (device.power)
            return `rgb(${hsv.rgb([device.color.h, device.color.s * 100, device.color.v * 100]).toString()})`;
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
                                {/*TODO сделать нормальный пикер, что-бы анимация смены цвета происходила при перетаскивании мышкой, а отправка на сервер после отжатия кнопки*/}
                                <PhotoshopPicker
                                    color={x.color}
                                    onChangeComplete={handleSetColor(x)}
                                />
                            </div>
                            <div>
                                <PowerIcon poweron={x.power}
                                           onClick={handleTogglePower(x)}/>
                            </div>
                        </DeviceStateControlsContainer>
                    </ExpansionPanelDetails>
                </ExpansionPanel>
            )}

        </DeviceListContainer>
    </Container>)
};

export default DevicesPage