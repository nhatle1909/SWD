import baseClient from "./baseClient";
export const getBlogList = ({pageIndex, isAsc, searchValue}) => {
    const response = baseClient.get(`/api/blog/get-paging-blog-list?pageIndex=${pageIndex}&isAsc=${isAsc}&searchValue=${searchValue}`);
    console.log("response", response);
    return response;
};
