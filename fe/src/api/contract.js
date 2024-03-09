import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const getContracts = (request) => {
    return baseClient.post('/Account/Admin/Get-Paging-Account-List', request);
};
