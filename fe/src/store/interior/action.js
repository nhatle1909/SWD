import { toast } from "react-toastify";
import { getInteriors } from "../../api/interior";
import {
  setInteriors,
} from "./slice";

export const actionGetInteriors = (request) => {
  return async (dispatch) => {
    try {
      const { data } = await getInteriors(request);

      console.log("data", data);

      dispatch(setInteriors(data));

    } catch (error) {
      toast(error.response.data, {
        type: 'error'
      });
      console.log(error)
      throw error;
    }
  };
}

import { getInteriorList } from '@/api/interior';
import { setDetailInterior, setInteriors } from './slice';
import { toast } from 'react-toastify';
import { getDetailInterior } from '../../api/interior';

export const actionGetInteriosList = ({pageIndex, isAsc, searchValue}) => {
    return async (dispatch) => {
        try {
            const {data} = await getInteriorList({ pageIndex, isAsc, searchValue });
            dispatch(setInteriors(data));
        } catch (error) {
            toast('Something went wrong, pls contact admin!', {
                type: 'error'
            })
            console.error('Error fetching blogs:', error);
        }
    };
};

export const actionGetDetailInterior = (interiorId) => {
  return async (dispatch) => {
      try {
          const {data} = await getDetailInterior(interiorId);
          dispatch(setDetailInterior(data));
      } catch (error) {
          toast('Something went wrong, pls contact admin!', {
              type: 'error'
          })
          console.error('Error fetching blogs:', error);
      }
  };
};


