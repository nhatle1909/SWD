import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const getInteriors = (request) => {
    console.log("request", request);
    return baseClient.post('/Interior/Get-Paging-Interior-List', request);
};

export const createInterior = (request) => {
    return baseClient.post('/Interior/Get-Paging-Interior-List', request);
};

