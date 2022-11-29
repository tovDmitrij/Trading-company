﻿const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

document.getElementById("acceptBtn").onclick = function () {
    const fullName = document.getElementById("fullName");
    const email = document.getElementById("email");
    const password = document.getElementById("password");
    const percent = document.getElementById("percent");

    if (!fullName.checkValidity()) {
        fullName.reportValidity();
        return false;
    }
    if (!email.checkValidity()) {
        email.reportValidity();
        return false;
    }
    if (!password.checkValidity()) {
        password.reportValidity();
        return false;
    }
    if (!percent.checkValidity()) {
        percent.reportValidity();
        return false;
    }

    hubConnection.invoke("SignUpCheck", email.value);
}

hubConnection.on("Message", function (err) {
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function () {
    document.getElementById("signUpForm").submit();
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });