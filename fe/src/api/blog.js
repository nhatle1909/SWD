import baseClient from "./baseClient";
export const getBlogList = ({pageIndex, isAsc, searchValue}) => {
    const response = baseClient.get(`/blog/get-paging-blog-list?pageIndex=${pageIndex}&isAsc=${isAsc}&searchValue=${searchValue}`);
    return response;
};
export const getBlogs = (request) => {
    return baseClient.get(`/Blog/Get-Paging-Blog-List`, request);
};
export const removeBlog = (request) => {
    console.log("request",request);
    return baseClient.delete(`/Blog/Staff/Remove-An-Blog`, {data: request});
};
export const createBlog = (request) => {
    const formData = new FormData();
    formData.append('Pictures', request.image);

    const title = encodeURIComponent(request.title);
    const content = encodeURIComponent(request.content);

    return baseClient.post(`/Blog/Staff/Add-An-Blog?Title=${title}&Content=${content}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    });
};

export const updateBlog = (request) => {
    console.log("Request",request);
    const formData = new FormData();
    if (request.pictures[0] instanceof Blob)
        formData.append("Pictures", request.pictures[0]);
    else {
        var byteString = atob(request.pictures[0]);
        var arrayBuffer = new ArrayBuffer(byteString.length);
        var _ia = new Uint8Array(arrayBuffer);
        for (var i = 0; i < byteString.length; i++)
            _ia[i] = byteString.charCodeAt(i);
        var dataView = new DataView(arrayBuffer);
        var blob = new Blob([dataView], {type: 'image/jpeg'});
        formData.append('Pictures', blob);
    }
    const title = encodeURIComponent(request.title);
    const content = encodeURIComponent(request.content);
    return baseClient.patch(`/Blog/Staff/Update-An-Blog?BlogId=${request.blogId}&Title=${title}&Content=${content}`,
                formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
            });
};