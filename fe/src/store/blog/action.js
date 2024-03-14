import { getBlogList } from '@/api/blog';
import { setBlogs } from './slice';
import { toast } from 'react-toastify';
import { createBlog, getBlogs, removeBlog } from '../../api/blog';
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

export const actionGetBlogs = (request) => {
    return async (dispatch) => {
        try {
            const { data } = await getBlogs(request);
            console.log("data", data);
            dispatch(setBlogs(data));
        } catch (error){
            toast(error.response.data,{ type : 'error'});
            console.log(error)
            throw error;
        }
    };
}

export const actionRemoveBlog = (request) => {
    return async (dispatch) => {
        try{
            await removeBlog(request);
            const { data } = await getBlogs({
                PageIndex: 1,
                IsAcs: true,
                SearchValue: ""
            });
            dispatch(setBlogs(data));
            toast('Remove blog successful', { type: 'success'});
        } catch (error) {
            toast(error.response.data, {
                type: 'error'
              });
              console.log(error)
              throw error;
        }
    };
}

export const actionAddBlog = (request) => {
    return async (dispatch) => {
      try {
        console.log("going to add: ", request)
        const response = await createBlog(request.blogType, { title: request.title, content: request.content });
        console.log("response", response);
        const { data } = await getBlogs({
          PageIndex: 1,
          IsAsc: true,
          SearchValue: "",
        });
  
        dispatch(setBlogs(data));
        toast('Add new Blog successful', {
          type: 'success'
        });
      } catch (error) {
        toast(error.response.data, {
          type: 'error'
        });
        throw error;
      }
    };
}