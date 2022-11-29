const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

document.getElementById("acceptBtn").onclick = function () {
    const contract_id = document.getElementById("selectContract");
    const prod_id = document.getElementById("selectProdID");
    const quantity = document.getElementById("inputQuantity");
    const transaction_date = document.getElementById("inputDate");

    if (!contract_id.checkValidity()) {
        contract_id.reportValidity();
        return false;
    }
    if (!prod_id.checkValidity()) {
        prod_id.reportValidity();
        return false;
    }
    if (!quantity.checkValidity()) {
        quantity.reportValidity();
        return false;
    }
    if (!transaction_date.checkValidity()) {
        transaction_date.reportValidity();
        return false;
    }

    hubConnection.invoke("SubmitSell", prod_id.value, quantity.value, transaction_date.value);
}

document.getElementById("checkBtn").onclick = function () {
    const prod_id = document.getElementById("selectProdID");
    const quantity = document.getElementById("inputQuantity");
    const transaction_date = document.getElementById("inputDate");

    if (!prod_id.checkValidity()) {
        prod_id.reportValidity();
        return false;
    }
    if (!quantity.checkValidity()) {
        quantity.reportValidity();
        return false;
    }
    if (!transaction_date.checkValidity()) {
        transaction_date.reportValidity();
        return false;
    }

    hubConnection.invoke("GetCheck", prod_id.value, quantity.value, transaction_date.value);
}

hubConnection.on("GiveCheck", function (cost, tax, totalCost, date_description = "") {
    document.getElementById("checkCost").innerHTML = cost;
    document.getElementById("checkTax").innerHTML = tax;
    document.getElementById("checkTotalCost").innerHTML = totalCost;
    document.getElementById("checkError").innerHTML = "";
    document.getElementById("date_description").innerHTML = date_description;
});

hubConnection.on("CheckError", function (err) {
    document.getElementById("checkCost").innerHTML = "---";
    document.getElementById("checkTax").innerHTML = "---";
    document.getElementById("checkTotalCost").innerHTML = "---";
    document.getElementById("checkError").innerHTML = err;
});

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function () {
    document.getElementById("transactionForm").submit();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });