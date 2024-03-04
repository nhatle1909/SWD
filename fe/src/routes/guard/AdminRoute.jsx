
import {  Outlet } from "react-router-dom";
import { getLocalStorage } from "../../utils/common";
import { jwtDecode } from 'jwt-decode'
const AdminRoute = () => {
  const auth = getLocalStorage('auth');
  if(auth?.token){
    const payload =  jwtDecode(auth.token)

    if(payload.role === 'Admin'){
      return <Outlet />
    }
  }
  return  window.location.href = '/'
};

export default AdminRoute;