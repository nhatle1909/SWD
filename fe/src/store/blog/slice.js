import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  blogs: null

};

export const blogsSlice = createSlice({
  name: "blogs",
  initialState,
  reducers: {
    setBlogs(state, action) {
      state.blogs = action.payload;
    },
  },
});

export const { 
  setBlogs,
} = blogsSlice.actions;

export default blogsSlice.reducer;