import {  Navigate, Outlet } from "react-router-dom";
import { getLocalStorage } from "../../utils/common";

// setup for the user role
const PermissionRoute = ({permissions}) => {
  if(!permissions){
    return <Outlet/>
  }
  const auth = getLocalStorage('auth');

  console.log(auth);
  if(permissions.includes('user')){
    if(auth?.token){
      return <Outlet/>
    }

    return window.location.href = '/'
  }
};

export default PermissionRoute;