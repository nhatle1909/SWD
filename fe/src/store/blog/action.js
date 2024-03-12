import { getBlogList } from '@/api/blog';
import { setBlogs } from './slice';
import { toast } from 'react-toastify';
// Định nghĩa một action type
export const SET_BLOGS = 'SET_BLOGS';

export const actionGetBlogList = ({pageIndex, isAsc, searchValue}) => {
    return async (dispatch) => {
        try {
            const {data} = await getBlogList({ pageIndex, isAsc, searchValue });
            dispatch(setBlogs(data));
        } catch (error) {
            toast('Something went wrong, pls contact admin!', {
                type: 'error'
            })
            console.error('Error fetching blogs:', error);
        }
    };
};
