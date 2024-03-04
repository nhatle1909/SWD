import baseClient from "./baseClient";
export const login = (email, password) => {
    const response = baseClient.post('/account/login-by-email-password', { email, password });
    console.log("response", response);
    return response;
};

export const signUpUser = (email, password) => {
    return baseClient.post('/account/register-customer-account', { email, password, confirmPassword: password });
};

export const signUpSeller = (email, password) => {
    return baseClient.post('/account/register-staff-account', { email, password });
};

export const sendMailResetPassword = (email) => {
    return baseClient.post('/account/send-mail-to-reset-password', { email });
};
