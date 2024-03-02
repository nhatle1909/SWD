import React from 'react'
import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import ReactDOM from 'react-dom/client'
import { injectStore } from "./api/baseClient";
import App from './App.jsx'
import { store } from "./store";
import '@/assets/css/open-iconic-bootstrap.min.css';
import '@/assets/css/animate.css';
import '@/assets/css/owl.carousel.min.css';
import '@/assets/css/owl.theme.default.min.css';
import '@/assets/css/magnific-popup.css';
import '@/assets/css/aos.css';
import '@/assets/css/ionicons.min.css';
import '@/assets/css/bootstrap-datepicker.css';
import '@/assets/css/jquery.timepicker.css';
import '@/assets/css/flaticon.css';
import '@/assets/css/icomoon.css';
import '@/assets/css/style.css';
import './index.scss';

// Inject store here to prevent circular import issue
injectStore(store);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </Provider>
  </React.StrictMode>,
)
