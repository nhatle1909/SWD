
import { sendMailResetPassword, signUpUser } from "../../api/auth";
import {
  setAuthUser,
} from "./slice";
import {
  login,
} from "@/api/auth";
import { setLocalStorage } from "../../utils/common";
import { jwtDecode } from "jwt-decode";
export const actionLogin = (
  { email, password }
) => {
  return async (dispatch) => {
    try {
      const { data } = await login(email, password);

      const token = data.message;
      console.log("token", token)

      const info = jwtDecode(token);
      dispatch(setAuthUser({ token: token, email: email, role: info.role }));
      setLocalStorage('auth', { token: token, email: email, role: info.role });
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

    } catch (error) {
      console.log(error)
      throw error;
    }
  };
}
