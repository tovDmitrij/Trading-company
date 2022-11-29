const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

document.getElementById("acceptBtn").onclick = function () {
    const email = document.getElementById("email");
    const password = document.getElementById("password");

    if (!email.checkValidity()) {
        email.reportValidity();
        return false;
    }
    if (!password.checkValidity()) {
        password.reportValidity();
        return false;
    }

    hubConnection.invoke("SignInCheck", email.value, password.value)
}

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function () {
    document.getElementById("signInForm").submit();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });