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

export const getInteriorList = ({pageIndex, isAsc, searchValue}) => {
    return baseClient.get(`/interior/get-paging-interior-list?pageIndex=${pageIndex}&isAsc=${isAsc}&searchValue=${searchValue}`);
};

export const getDetailInterior = (interiorId) => {
    return baseClient.get(`/interior/view-detail-interior-from-paging?interiorId=${interiorId}`);
};
