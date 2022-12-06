//#region Конвертация даты к формату dd-MM-yyyy

function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function formatDate(date) {
    return [
        padTo2Digits(date.getDate()),
        padTo2Digits(date.getMonth() + 1),
        date.getFullYear(),
    ].join('-');
}

//#endregion



//#region Подключение к хабу и взаимодействие с ним

const hubConnection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

hubConnection.on("Message", function (err) {
    $("#priceBodyTable").empty();
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function (priceList) {
    $("#priceBodyTable").empty();
    document.getElementById("Message").innerHTML = "";

    for (var i = 0; i < priceList.length; i++) {
        var dValue = 0;
        if (i != 0) {
            dValue = priceList[i]["price_value"] - priceList[i - 1]["price_value"];
        }
        else {
            dValue = 0;
        }

        var trend;

        if (dValue == 0.0) {
            trend = "minus.png";
        }
        else if (dValue > 0.0) {
            trend = "up.png";
        }
        else {
            trend = "down.png";
        }

        var currentTR = '<tr>\
                            <td>'+ priceList[i]["product_name"] +'#'+ priceList[i]["product_id"] +'</td>\
                            <td>'+ formatDate(new Date(priceList[i]["price_dayfrom"])) +'</td>\
                            <td>'+ formatDate(new Date(priceList[i]["price_dayto"])) + '</td>\
                            <td>'+ Number((priceList[i]["price_value"]).toFixed(2)) +' (руб.)</td>\
                            <td>\
                                <div style="text-align: center;" class="row">\
                                    <div style="text-align: right; display: inline-block;" class="col-6">\
                                        <img width="15px" height="15px" src="/images/tendentions/'+ trend.toString() +'" />\
                                    </div>\
                                    <div style="text-align: left; display: inline-block;" class="col-6">\
                                        '+ Math.abs(Number((dValue).toFixed(2))) +' (руб.)\
                                    </div>\
                                </div>\
                            </td>\
                        </tr>';
        $('#priceBodyTable').append(currentTR);
    }
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });

//#endregion



//#region Получение курса выбранной валюты за последний месяц

function GetPriceValues(courseID) {
    hubConnection.invoke("GetPriceValues", courseID);
}

//#endregion