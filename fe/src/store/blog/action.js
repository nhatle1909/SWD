import { getBlogList } from '@/api/blog';

// Định nghĩa một action type
export const SET_BLOGS = 'SET_BLOGS';

export const actionGetBlogList = ({pageIndex, isAsc, searchValue}) => {
    return async (dispatch) => {
        try {
            const response = await getBlogList({ pageIndex, isAsc, searchValue });
            console.log('Response data:', response.data);
            if (response.data) {
                // Dispatch một action để lưu trữ dữ liệu blogs vào Redux store
                dispatch({ type: SET_BLOGS, payload: response.data });
            }
        } catch (error) {
            console.error('Error fetching blogs:', error);
        }
    };
};
