//#region Поиск чего-либо по поисковой строке (https://www.w3schools.com/howto/howto_js_filter_table.asp)
function searchContract(input, table, index) {
    document.getElementById("checkboxFilter").checked = false;

    var filter, tr, td, i, j;
    filter = input.value.toUpperCase();
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[index];
        if (td) {
            if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            }
            else {
                tr[i].style.display = "none";
            }
        }
    }
}
//#endregion

//#region Фильтрация списка по чекбоксу (https://codepen.io/carl_was_here/pen/QoGRBp)
function filter_type(box) {
    document.getElementById("textFilter").value = "";

    var cbs = document.getElementsByTagName('input');
    var all_checked_types = [];
    for (var i = 0; i < cbs.length; i++) {
        if (cbs[i].type == "checkbox") {
            if (cbs[i].name.match(/^filter/)) {
                if (cbs[i].checked) {
                    all_checked_types.push(cbs[i].value);
                }
            }
        }
    }
    if (all_checked_types.length > 0) {
        $('#contractsTable tr').each(function (i, row) {
            var $tds = $(this).find('td')
            if ($tds.length) {
                var type = $tds[3].innerText;
                var [day, month, year] = type.split('-');
                var comparableDate = new Date(+year, +month - 1, +day);

                [day, month, year] = all_checked_types[0].split('-');
                var currentDate = new Date(+year, +month - 1, +day);

                if (comparableDate <= currentDate) {
                    $(this).hide();
                }
                else {
                    $(this).show();
                }
            }
        });
    }
    else {
        $('#contractsTable tr').each(function (i, row) {
            var $tds = $(this).find('td'),
                type = $tds.eq(2).text();
            $(this).show();
        });
    }
    return true;
}//#endregion