import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const getAccounts = (request) => {
    return baseClient.post('/Account/Admin/Get-Paging-Account-List', request);
};

export const viewAccount = (request) => {
    return baseClient.get('/Account/Admin/View-Profile?email=' + request);
};

export const createAccountStaff = (request) => {
    return baseClient.post('/Account/Admin/Create-Staff-Account', request);
};

export const createAccountCustomer = (accountType, request) => {

    if (accountType === "customer") {
        return baseClient.post('/Account/Create-Customer-Account', request);
    }

    return baseClient.post('/Account/Create-Staff-Account', request);

};

export const removeAccount = (request) => {
    console.log("going to delete: ", request);

    return baseClient.post('/Account/Admin/Remove-Account', request);
};