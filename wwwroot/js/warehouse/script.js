//#region JS-код, необходимый для отрисовки диаграммы (основано на https://code.tutsplus.com/ru/tutorials/how-to-draw-a-pie-chart-and-doughnut-chart-using-javascript-and-html5-canvas--cms-27197)


//Генерация случайного цвета для некоторой части диаграммы
function generateColor() {
    const randomColor = Math.floor(Math.random() * 16777215).toString(16);
    return "#" + randomColor;
}


//Холст для диаграммы
var myCanvas = document.getElementById("myCanvas");
myCanvas.width = 600;
myCanvas.height = 600;
var ctx = myCanvas.getContext("2d");


//Список групп товаров
var productGroupList = {};
var warehouseList = document.getElementById('warehouseTable');
for (i = 1; i < warehouseList.rows.length; i++) {
    var currentProduct = warehouseList.rows.item(i).cells;

    var productGroup = currentProduct.item(1).innerHTML;
    var productQuantity = Number(currentProduct.item(2).innerHTML);
    productGroupList["" + productGroup] = (productGroupList[productGroup] || 0) + productQuantity;
}


//Генерация цветов для отображения
const myColors = [];
for (let i = 0; i < Object.keys(productGroupList).length; i++, myColors.push(generateColor()));


var Piechart = function (options) {
    this.options = options;
    this.canvas = options.canvas;
    this.ctx = this.canvas.getContext("2d");
    this.colors = options.colors;

    this.draw = function () {
        var total_value = 0;
        var color_index = 0;
        for (var categ in this.options.data) {
            var val = this.options.data[categ];
            total_value += val;
        }

        var start_angle = 0;
        for (categ in this.options.data) {
            val = this.options.data[categ];
            var slice_angle = 2 * Math.PI * val / total_value;

            drawPieSlice(
                this.ctx,
                this.canvas.width / 2,
                this.canvas.height / 2,
                Math.min(this.canvas.width / 2, this.canvas.height / 2),
                start_angle,
                start_angle + slice_angle,
                this.colors[color_index % this.colors.length]
            );

            start_angle += slice_angle;
            color_index++;
        }
        if (this.options.doughnutHoleSize) {
            drawPieSlice(
                this.ctx,
                this.canvas.width / 2,
                this.canvas.height / 2,
                this.options.doughnutHoleSize * Math.min(this.canvas.width / 2, this.canvas.height / 2),
                0,
                2 * Math.PI,
                "#ffffff"
            );
        }
        start_angle = 0;
        for (categ in this.options.data) {
            val = this.options.data[categ];
            slice_angle = 2 * Math.PI * val / total_value;
            var pieRadius = Math.min(this.canvas.width / 2, this.canvas.height / 2);
            var labelX = this.canvas.width / 2 + (pieRadius / 2) * Math.cos(start_angle + slice_angle / 2);
            var labelY = this.canvas.height / 2 + (pieRadius / 2) * Math.sin(start_angle + slice_angle / 2);

            if (this.options.doughnutHoleSize) {
                var offset = (pieRadius * this.options.doughnutHoleSize) / 2;
                labelX = this.canvas.width / 2 + (offset + pieRadius / 2) * Math.cos(start_angle + slice_angle / 2);
                labelY = this.canvas.height / 2 + (offset + pieRadius / 2) * Math.sin(start_angle + slice_angle / 2);
            }
            if (val != 0) {
                var labelText = Math.round(100 * val / total_value);
                this.ctx.fillStyle = "white";
                this.ctx.font = "1000 20px Arial";
                this.ctx.fillText(val + "шт. (" + labelText + "%)", labelX, labelY);
            }
            start_angle += slice_angle;
        }
        if (this.options.legend) {
            color_index = 0;
            var legendHTML = "";
            for (categ in this.options.data) {
                legendHTML += "<div><span style='display:inline-block;width:20px;background-color:" + this.colors[color_index++] + ";'>&nbsp;</span> " + categ + "</div>";
            }
            this.options.legend.innerHTML = legendHTML;
        }
    }
}

function drawLine(ctx, startX, startY, endX, endY) {
    ctx.beginPath();
    ctx.moveTo(startX, startY);
    ctx.lineTo(endX, endY);
    ctx.stroke();
}

function drawArc(ctx, centerX, centerY, radius, startAngle, endAngle) {
    ctx.beginPath();
    ctx.arc(centerX, centerY, radius, startAngle, endAngle);
    ctx.stroke();
}

function drawPieSlice(ctx, centerX, centerY, radius, startAngle, endAngle, color) {
    ctx.fillStyle = color;
    ctx.beginPath();
    ctx.moveTo(centerX, centerY);
    ctx.arc(centerX, centerY, radius, startAngle, endAngle);
    ctx.closePath();
    ctx.fill();
}

var myLegend = document.getElementById("myLegend");
var myDougnutChart = new Piechart(
    {
        canvas: myCanvas,
        data: productGroupList,
        colors: myColors,
        legend: myLegend
    }
);
myDougnutChart.draw();
//#endregion