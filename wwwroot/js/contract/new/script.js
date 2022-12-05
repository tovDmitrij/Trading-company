const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();


document.getElementById("acceptBtn").onclick = function () {
    const contragent_id = document.getElementById("contr_id");
    const contract_dayto = document.getElementById("dayto");

    if (!contragent_id.checkValidity()) {
        contragent_id.reportValidity();
        return false;
    }
    if (!contract_dayto.checkValidity()) {
        contract_dayto.reportValidity();
        return false;
    }

    hubConnection.invoke("CheckBankAccount", contragent_id.value, contract_dayto.value);
}

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function () {
    document.getElementById("ContractForm").submit();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });