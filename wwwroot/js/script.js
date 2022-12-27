//#region Поиск чего-либо по поисковой строке (основано на https://www.w3schools.com/howto/howto_js_filter_table.asp)
function searchByText(input, table, index) {
    var chckBx = document.getElementById("checkboxFilter");
    if (chckBx != null) {
        chckBx.checked = false;
    }

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


//#region Фильтрация списка по дате (чекбокс) (основано на https://codepen.io/carl_was_here/pen/QoGRBp)
function searchByCheckBox(index, table) {
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

    var tableName = "#" + table + " tr";
    if (all_checked_types.length > 0) {
        $(tableName).each(function (i, row) {
            var $tds = $(this).find('td')
            if ($tds.length) {
                var type = $tds[index].innerText;
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
        $(tableName).each(function (i, row) {
            var $tds = $(this).find('td'),
                type = $tds.eq(2).text();
            $(this).show();
        });
    }
    return true;
}//#endregion