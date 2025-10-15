import api from "axios";

export async function getMyInfo() {
    const response = await api.post(`/authentication/me`);
    return response.data;
}