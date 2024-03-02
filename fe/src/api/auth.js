import baseClient from "./baseClient";
export const login = (email, password) => {
    return baseClient.post('/auth/login',{ email, password });
};