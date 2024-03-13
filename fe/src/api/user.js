import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const getAccounts = (request) => {
    console.log(request);
    return baseClient.post('/Account/Get-Paging-Account-List', request);
};

export const viewAccount = (request) => {
    return baseClient.get('/Account/View-Profile?email=' + request);
};

export const createAccountStaff = (request) => {
    return baseClient.post('/Account/Register-Staff-Account', request);
};

export const createAccountCustomer = (request) => {
    return baseClient.post('/Account/Register-Customer-Account', request);
};

export const removeAccount = (request) => {
    return baseClient.delete('/Account/Remove-Account', request);
};

export const changeAvatarProfile = (file) => {
    const formData = new FormData();
    formData.append('picture', file);
    return baseClient.patch('/account/authorize/update-picture-account', formData);
}

export const uploadInfoProfile = (phoneNumber, homeAddress) => {
    return baseClient.patch('/account/authorize/update-an-account', {
        phoneNumber,
        homeAdress: homeAddress
    });
}
