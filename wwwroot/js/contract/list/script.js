const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

function EndContract(contract_id) {
    const contract_date = document.getElementById("dateValue-" + contract_id);

    if (!contract_date.checkValidity()) {
        contract_date.reportValidity();
        return false;
    }

    hubConnection.invoke("EndEarlyContract", contract_id, contract_date.value);
}

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function (contract_id, contract_date) {
    var contractForm = document.getElementById("contractForm-" + contract_id);

    contractForm.submit();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });