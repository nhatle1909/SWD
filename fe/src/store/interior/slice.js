import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  interiors: [],
};

export const interiorSlice = createSlice({
  name: "interior",
  initialState,
  reducers: {
    setInteriors(state, action) {
      state.interiors = action.payload;
    },
  },
});

export const {
  setInteriors
} = interiorSlice.actions;

export default interiorSlice.reducer;