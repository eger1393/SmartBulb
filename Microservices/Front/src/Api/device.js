import axios from 'axios'

export const apiGetDeviceList = () =>
    axios.get(`api/device/list`).then(data => data.data);

export const apiGetDeviceState = (deviceId) =>
    axios.get(`api/device/${deviceId}`).then(data => data.data);

export const apiSetDeviceState = (deviceId, state) =>
    axios.post(`api/device/${deviceId}/setState`, state).then(data => data.data);
