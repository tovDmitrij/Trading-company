const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

function EndContract(contract_id) {
    //const contract_id = document.getElementById("acceptBtn").value;

    hubConnection.invoke("EndEarlyContract", contract_id)
}

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function (contract_id) {
    var contractForm = document.getElementById("contractForm");

    var newInput = document.createElement("button");
    newInput.setAttribute("type", "submit");
    newInput.setAttribute("name", "id")
    newInput.setAttribute("id", "contract_id");
    newInput.setAttribute("value", contract_id);
    newInput.setAttribute("class", "btn");

    contractForm.appendChild(newInput);
    newInput.click();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });