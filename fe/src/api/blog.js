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