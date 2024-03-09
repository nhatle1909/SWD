
import { toast } from "react-toastify";
import { getContracts } from "../../api/contract";
import {
  setContracts,
} from "./slice";

export const actionGetContracts = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getContracts(request);

      console.log("data", data);

      dispatch(setContracts(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}
