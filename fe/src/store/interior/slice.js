import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  interiors: null,
  interior: null
};

export const interiorsSlice = createSlice({
  name: "interior",
  initialState,
  reducers: {
    setInteriors(state, action) {
      state.interiors = action.payload;
    },
     setDetailInterior(state, action){
      state.interior = action.payload;
    },
  },
});

export const { 
  setInteriors,
  setDetailInterior
} = interiorsSlice.actions;

export default interiorsSlice.reducer;
