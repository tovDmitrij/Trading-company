const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

function CancelBuyTransaction(transaction_id) {
    hubConnection.invoke("DeleteBuyTransaction", transaction_id);
}

function CancelSellTransaction(transaction_id) {
    hubConnection.invoke("DeleteSellTransaction", transaction_id);
}

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("SubmitDeleteBuy", function (transaction_id) {
    var transactionForm = document.getElementById("TransactionBuy-" + transaction_id);

    transactionForm.submit();
});

hubConnection.on("SubmitDeleteSell", function (transaction_id) {
    var transactionForm = document.getElementById("TransactionSell-" + transaction_id);

    transactionForm.submit();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });