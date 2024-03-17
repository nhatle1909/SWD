import baseClient from "./baseClient";

export const getInfo = () => {
    return baseClient.get('/user');
};

export const getInteriors = (request) => {
    return baseClient.post('/Interior/Get-Paging-Interior', request);
};

export const createInterior = (request, file) => {
    const formData = new FormData();
    formData.append('Image', file);

    const params = new URLSearchParams();
    params.append('InteriorName', request.interiorName);
    params.append('InteriorType', request.interiorType);
    params.append('Description', request.description);
    params.append('Quantity', request.quantity);
    params.append('Price', request.price);

    return baseClient.post(`/Interior/Staff/Add-New-Interior?${params.toString()}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

export const getInteriorList = ({ pageIndex, isAsc, searchValue }) => {
    return baseClient.get(`/interior/get-paging-interior-list?pageIndex=${pageIndex}&isAsc=${isAsc}&searchValue=${searchValue}`);
};

export const getDetailInterior = (interiorId) => {
    return baseClient.get(`/interior/view-detail-interior-from-paging?interiorId=${interiorId}`);
};

export const removeInterior = (request) => {
    return baseClient.delete(`/Interior/Staff/Delete-Interior`, {data: request});
}

export const updateInterior = (request, file) => {
    const formData = new FormData();
    formData.append('Image', file);

    const params = new URLSearchParams();
    params.append('InteriorId', request.interiorId);
    params.append('InteriorName', request.interiorName);
    params.append('Quantity', 100)
    params.append('Price', request.price);

    return baseClient.put(`/Interior/Staff/Update-Interior?${params.toString()}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}