
import { changePassword, sendMailResetPassword, signUpUser, resetPassword, getUserInfo } from "../../api/auth";
import {
  setAuthUser,
} from "./slice";
import {
  login,
} from "@/api/auth";
import { setLocalStorage } from "../../utils/common";
import { jwtDecode } from "jwt-decode";
import {toast} from 'react-toastify'
export const actionLogin = (
  { email, password }
) => {
  return async (dispatch) => {
    try {
      const { data } = await login(email, password);  
      const info = jwtDecode(data);
      dispatch(setAuthUser({token:data, email: email, role: info.role}));
      setLocalStorage('auth', {token: data, email: email, role: info.role});
    } catch (error) {
      console.log(error)
      toast('Password or email was wrong, pls try again!', {
        type:'error'
      })
      throw error;
    }
  };
};

export const actionSignUpUser = ({ email, password, phoneNumber }) => {
  return async (dispatch) => {
    try {
      await signUpUser(email, password, phoneNumber);
      const { data } = await login(email, password);
      const info = jwtDecode(data);
      dispatch(setAuthUser({ token: data, email: email, role: info.role }));
      setLocalStorage('auth', { token: data, email: email, role: info.role });

    } catch (error) {
      console.log(error)
      throw error;
    }
  };
}

export const actionSendMailToResetPassword = (email) => {
  return async (dispatch) => {
    try {
      await sendMailResetPassword(email);
      toast('Link reset password sent to your email, pls check it!', {
        type: 'success'
      })
      return
    } catch (error) {
      console.log(error)
      throw error;
    }
  };
}

export const actionResetPassword = (token, pass) => {
  return async () => {
    try {
      await resetPassword(token, pass);
      toast('Password was changed, pls login again!', {
        type: 'success'
      })

      
      return 'reset-password'
    } catch (error) {
      console.log(error)
      toast('Link reset password is invalid, pls try again!', {
        type: 'error'
      })
      throw error;
    }
  };
}

export const actionChangePassword = (oldPass, newPass) => {
  return async () => {
    try {
      await changePassword(oldPass, newPass)
      toast('New password updated!', {
        type:'success'
      })
      return 'success'
    } catch (error) {
      console.log(error)
      toast('The old password was wrong!, please try again!', {
        type:'error'
      })
      throw error;
    }
  };
}

export const actionGetUserInfo = () => {
  return async () => {
    try {
      const {data} = await getUserInfo()
      return data
    } catch (error) {
      console.log(error)
      toast('Something went wrong!, please try again!', {
        type:'error'
      })
      throw error;
    }
  };
}

