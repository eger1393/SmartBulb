import axios from 'axios'

export const configureAxios = (history) => {
    axios.interceptors.request.use(config => {
        config.headers["Authorization"] = "bearer " + getAuthToken();
        return config;
    });

    axios.interceptors.response.use(response => response,
        error => {
            if(error.response.status === 401){
                localStorage.removeItem('currentUser');
                history.push("/")
            }
            return Promise.reject(error);
        })
};


const getAuthToken = () => {
    return localStorage.getItem('currentUser');
};

