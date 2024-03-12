
import { toast } from "react-toastify";
import { createRequest, getRequests } from "../../api/request";

import {
  setRequests,
} from "./slice";

export const actionGetRequests = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getRequests(request);

      console.log("data", data);

      dispatch(setRequests(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}

export const createRequestAction = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await createRequest(request);
      toast(data, {
        type: 'success'
      });
    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}
