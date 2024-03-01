import baseClient from "./baseClient";
export const getInfo = () => {
    return baseClient.get('/user');
};