import baseClient from "./baseClient";

export const getTransactions = (request) => {
    return baseClient.post('/Transaction/Customer/Get-All-Transaction');
};

export const getTransaction = (request) =>
    baseClient.post(`/Transaction/Customer/Get-Transaction-Detail?transactionId=${request.transactionId}`);
