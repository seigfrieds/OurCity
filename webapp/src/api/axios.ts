import axios from "axios";

const config = function axiosConfig() {
    return {
        baseUrl: "http://localhost:8000/api/v1", 
        headers: {},
    };
}

const api = axios.create(config());

export default api;