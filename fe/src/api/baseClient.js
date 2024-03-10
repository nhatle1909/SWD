
import axios from "axios";
import { toast } from 'react-toastify';
import { getLocalStorage, setLocalStorage } from "../utils/common";

let store;

const BASE_URL = import.meta.env.VITE_APP_BASE_URL;
console.log(BASE_URL)
const baseClient = axios.create({
  baseURL: BASE_URL,
  withCredentials: false
});

export const injectStore = (_store) =>
  (store = _store);

baseClient.interceptors.response.use((response) => {
   return response;
}, error => {
  console.log('chekc token::', error)
  if( error.response.status === 401) {
    setLocalStorage('auth', {})
    toast('Token is expired, Please login again!', {
      type:'warning'
    })
    return Promise.reject(error)
  }
  return Promise.reject(error)
});

baseClient.interceptors.request.use((config) => {
  const auth = getLocalStorage('auth')
  config.headers.Authorization = "Bearer "+auth?.token;
  return config;
})

export default baseClient;