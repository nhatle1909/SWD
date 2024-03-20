import React from 'react';
import CardDataStats from '../../components/CardDataStats';
import ChartOne from '../../components/Charts/ChartOne';
import ChartTwo from '../../components/Charts/ChartTwo';
import PageHeader from '../../components/PageHeader';


const Reports = () => {
    React.useEffect(() => {
        window.location.href = 'https://sandbox.vnpayment.vn/merchantv2/Users/Login.htm?ReturnUrl=%2fmerchantv2%2fHome%2fDashboard.html'; // Redirect to the desired URL
      }, []);
   
};

export default Reports;
