import { getLocalStorage } from "../utils/common";
import baseClient from "./baseClient";
export const login = (email, password) => {
    console.log(email, password);
    const response = baseClient.post('/account/login-by-email-password', { email, password });
    console.log("respone", response);
    return response;
};

export const signUpUser = (email, password, phoneNumber) => {
    return baseClient.post('/account/create-customer-account',{ email, password, phoneNumber });
};

export const signUpSeller = (email, password) => {
    return baseClient.post('/account/register-staff-account', { email, password });
};

export const sendMailResetPassword = (email) => {
    return baseClient.post(`/account/send-mail-to-reset-password?email=${email}`);
};

export const resetPassword = (token, password) => {
    return baseClient.post(`/account/reset-password`, {
        token, password
    });
};

export const changePassword = (oldPass, newPass) => {
    return baseClient.patch('/account/authorize/change-password', {
        oldPassword: oldPass,
        password: newPass,
        confirmPassword: newPass
      });
};

export const getUserInfo = () => {
    return baseClient.get(`/account/view-public-profile?email=${getLocalStorage('auth').email}`);
};