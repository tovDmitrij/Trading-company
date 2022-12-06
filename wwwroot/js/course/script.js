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
    $("#courseBodyTable").empty();
    document.getElementById("Message").innerHTML = err;
});

hubConnection.on("Submit", function (courseValueList) {
    $("#courseBodyTable").empty();
    document.getElementById("Message").innerHTML = "";

    for (var i = 0; i < courseValueList.length; i++) {
        var dValue = 0;
        if (i != 0) {
            dValue = courseValueList[i]["value"] - courseValueList[i - 1]["value"];
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
                            <td>'+ courseValueList[i]["cur_namefrom"] +' -> '+ courseValueList[i]["cur_nameto"] +'</td>\
                            <td>'+ formatDate(new Date(courseValueList[i]["dayto"])) +'</td>\
                            <td>'+ courseValueList[i]["value"] +'</td>\
                            <td>\
                                <div class="row">\
                                    <div class="col-6 d-flex justify-content-end">\
                                        <img width="15px" height="15px" src="/images/tendentions/'+ trend.toString() +'" />\
                                    </div>\
                                    <div class="col-6 d-flex justify-content-start">\
                                        '+ Math.abs(Number((dValue).toFixed(2))) +'\
                                    </div>\
                                </div>\
                            </td>\
                        </tr>';
        $('#courseBodyTable').append(currentTR);
    }
});

hubConnection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });

//#endregion



//#region Получение курса выбранной валюты за последние 14 дней

function GetCourseValues(courseID) {
    hubConnection.invoke("GetCourseValues", courseID);
}

//#endregion