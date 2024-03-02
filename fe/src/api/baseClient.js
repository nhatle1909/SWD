
import axios from "axios";
import {setAuthUser} from '../store/authentication/slice'

let store;

const BASE_URL = import.meta.env.VITE_APP_BASE_URL;
console.log(BASE_URL)
const baseClient = axios.create({
  baseURL: BASE_URL,
  withCredentials: true
});

export const injectStore = (_store) =>
  (store = _store);

baseClient.interceptors.response.use((response) => {
   return response;
}, error => {
  console.log('err:::', error)
  if( error.response.status === 401) {
    return baseClient
    .post("/auth/login/refresh", {
      token: JSON.parse(localStorage.getItem('auth') 
    ).data.refreshToken,
    })
    .then(async ({ data }) => {
      await store.dispatch(setAuthUser(data))
      localStorage.setItem('auth', JSON.stringify(data))
      return baseClient(error.config);
    }).catch((err) => {
      console.log('err refresh', err)
    });
  }else{
    return Promise.reject(error)
  }
});

baseClient.interceptors.request.use((config) => {
  const token = store.getState().authentication.authUser?.data.token;
  console.log('test::', store.getState())
  config.headers.Authorization = "Bearer "+token;
  return config;
})

export default baseClient;