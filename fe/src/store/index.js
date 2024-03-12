import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";

import authenticationSlice from "./auth/slice";
import userSlice from "./user/slice";
import blogsSlice  from "./blog/slice";
import interiorsSlice from "./interior/slice";

export const store = configureStore({
  reducer: {
    authentication: authenticationSlice,
    user: userSlice,
    blogs: blogsSlice,
    interiors: interiorsSlice
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: false,
    }),
});

export const useAppDispatch = () => useDispatch();
export const useAppSelector = useSelector;
