import axios from 'axios'

export const apiAuthorize = (login, password) =>
    axios.post(`api/authorize`, {login, password}).then(resp => resp.data);
