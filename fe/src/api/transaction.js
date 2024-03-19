import baseClient from "./baseClient";

export const getTransactions = (request) => {
    return baseClient.post('/Transaction/Customer/Get-All-Transaction');
};

