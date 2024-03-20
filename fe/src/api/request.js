import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const createRequest = (request) => {
    console.log("going to add", request);
    return baseClient.post('/Contact/Add-An-Request-For-Guest', request);
};

export const getRequests = (request) => {
    return baseClient.post('/Contact/Get-Paging-Request-List', request);
};


export const createContactPDF = (request) => {
    return baseClient.post('/Contact/Staff/Create-Contract-PDF', request);
};
export const responseRequest =  ({id, response, status, file}) => {
    const form = new FormData();
    form.append("responseOfStaffInFile",file);
    return baseClient.post(`/Contact/Staff/Address-An-Request?RequestId=${id}&ResponseOfStaff=${response}&StatusResponseOfStaff=${status}`,form);
};