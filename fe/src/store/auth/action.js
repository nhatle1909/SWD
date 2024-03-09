
import { changePassword, sendMailResetPassword, signUpUser } from "../../api/auth";
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
      dispatch(setAuthUser({ token: data, email: email, role: info.role }));
      setLocalStorage('auth', { token: data, email: email, role: info.role });
    } catch (error) {
      console.log(error)
      throw error;
    }
  };
};

export const actionSignUpUser = ({ email, password }) => {
  return async (dispatch) => {
    try {
      await signUpUser(email, password);
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
      throw error;
    }
  };
}
