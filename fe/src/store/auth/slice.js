import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  authUser: null,
  picture:null
};

export const authenticationSlice = createSlice({
  name: "authentication",
  initialState,
  reducers: {
    setAuthUser(state, action) {
      state.authUser = action.payload;
    },
    setPicture(state, action){
      state.picture = action.payload;
    }
  },
});

export const { 
  setAuthUser,
  setPicture
} = authenticationSlice.actions;

export default authenticationSlice.reducer;