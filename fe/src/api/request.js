import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const createRequest = (request) => {
    console.log("going to add", request);
    return baseClient.post('/Contact/Add-An-Contact-For-Guest', request);
};

export const getRequests = (request) => {
    return baseClient.post('/Contact/Get-Paging-Contact-List', request);
};


export const createContactPDF = (request) => {
    return baseClient.post('/Contact/Staff/Create-Contract-PDF', request);
};