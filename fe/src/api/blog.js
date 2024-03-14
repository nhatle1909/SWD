import baseClient from "./baseClient";
export const getBlogList = ({pageIndex, isAsc, searchValue}) => {
    const response = baseClient.get(`/blog/get-paging-blog-list?pageIndex=${pageIndex}&isAsc=${isAsc}&searchValue=${searchValue}`);
    return response;
};
export const getBlogs = (request) => {
    return baseClient.get(`/Blog/Get-Paging-Blog-List`, request);
};
export const removeBlog = ({blogId}) => {
    return baseClient.delete(`/Blog/Staff/Remove-An-Blog?blogId=${blogId}`);
};
export const createBlog = (request) => {
    return baseClient.post('/Blog/Staff/Add-An-Blog',request);
};