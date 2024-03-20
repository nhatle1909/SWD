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

export const updateInterior = (request) => {    
    console.log("Request",request);
    const formData = new FormData();
    if (request.image instanceof Blob)
        formData.append("Image", request.image);
    else {
        var byteString = atob(request.image);
        var arrayBuffer = new ArrayBuffer(byteString.length);
        var _ia = new Uint8Array(arrayBuffer);
        for (var i = 0; i < byteString.length; i++)
            _ia[i] = byteString.charCodeAt(i);
        var dataView = new DataView(arrayBuffer);
        var blob = new Blob([dataView], {type: 'image/jpeg'});
        formData.append('Image', blob);
    }

    const params = {
        'InteriorId': request.interiorId,
        'InteriorName': encodeURIComponent(request.interiorName),
        'InteriorType': encodeURIComponent(request.interiorType),
        'Description': encodeURIComponent(request.description),
        'Quantity': encodeURIComponent(request.quantity),
        'Price': encodeURIComponent(request.price)
    };
    const queryString = Object.keys(params).map(key => `${key}=${params[key]}`).join('&');

    return baseClient.put(`/Interior/Staff/Update-Interior?${queryString}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
}