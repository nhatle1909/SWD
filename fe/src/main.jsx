import React from 'react'
import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import ReactDOM from 'react-dom/client'
import { injectStore } from "./api/baseClient";
import App from './App.jsx'
import { store } from "./store";
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
