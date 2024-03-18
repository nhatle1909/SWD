
import { toast } from "react-toastify";
import { createRequest, getRequests,responseRequest } from "../../api/request";

import {
  setRequests,
} from "./slice";
import { getLocalStorage } from "../../utils/common";

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
export const actionResponseRequest = ({id, response, status, file}) =>
{
  return async (dispatch) => {
    try {
      
      const { data } = await responseRequest({id, response, status, file});

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

export const actionGetMyRequests = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getRequests(request);
      const auth = getLocalStorage("auth");
      console.log("auth local", auth);
      console.log("filter data", data);

      const filter = data.filter(item => item.email === auth?.email);
      console.log("filter", filter);

      dispatch(setRequests(filter));

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

export const actionCreateContactPDF = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await createContactPDF(request);

      console.log("data", data);
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

