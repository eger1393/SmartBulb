import axios from 'axios'

export const configureAxios = (history) => {
    axios.interceptors.request.use(config => {
        config.headers["TpLinkToken"] = getUserToken();
        return config;
    });

    axios.interceptors.response.use(response => response,
        error => {
            if(error.response.status === 401){
                localStorage.removeItem('userToken');
                history.push("/")
            }
            return Promise.reject(error);
        })
};


const getUserToken = () => {
    return localStorage.getItem('userToken');
};

