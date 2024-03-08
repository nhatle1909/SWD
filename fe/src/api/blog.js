import baseClient from "./baseClient";
export const getBlogList = ({pageIndex, isAsc, searchValue}) => {
    const response = baseClient.get(`/api/blog/get-paging-blog-list?pageIndex=${pageIndex}&isAsc=${dsds}&searchValue=${searchValue}`, {});
    console.log("response", response);
    return response;
};

// export const addComment = (blogId, ) => {
//     return baseClient.post('/account/register-customer-account', { email, password, confirmPassword: password });
// };

// export const signUpSeller = (email, password) => {
//     return baseClient.post('/account/register-staff-account', { email, password });
// };

// export const sendMailResetPassword = (email) => {
//     return baseClient.post('/account/send-mail-to-reset-password', { email });
// };
