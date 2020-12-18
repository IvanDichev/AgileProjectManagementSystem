function sumArrayUpTo(arrData, index) {
    var total = 0;
    for (var i = 0; i <= index; i++) {
        if (arrData.length > i) {
            total += arrData[i];
        }
    }
    return total;
}

// Construct burndown chart
function showBurnDown(elementId, burndownData, scopeChange = [], daysInSprint) {

    var speedCanvas = document.getElementById(elementId);

    Chart.defaults.global.defaultFontFamily = "Arial";
    Chart.defaults.global.defaultFontSize = 14;
    const totalTasksInSprint = burndownData[0];
    console.log(burndownData[0])
    const idealHoursPerDay = totalTasksInSprint / (daysInSprint.length - 1);
    var data = [];
    for (var i = 0; i < daysInSprint.length; i++) {
        data.push(Math.round(totalTasksInSprint - (idealHoursPerDay * i) + sumArrayUpTo(scopeChange, i)))
    }

    var speedData = {
        labels: daysInSprint, // array of days eg. [1, 2, 3, 4, 5, 6]
        datasets: [
            {
                label: "Burndown",
                data: burndownData,
                fill: false,
                borderColor: "#EE6868",
                backgroundColor: "#EE6868",
                lineTension: 0,
            },
            {
                label: "Ideal",
                borderColor: "#6C8893",
                backgroundColor: "#6C8893",
                lineTension: 0,
                borderDash: [5, 5],
                fill: false,
                data: data
            },
        ]
    };

    var chartOptions = {
        legend: {
            display: true,
            position: 'top',
            labels: {
                boxWidth: 80,
                fontColor: 'black'
            }
        },
        scales: {
            yAxes: [{
                ticks: {
                    min: 0,
                    max: Math.round(burndownData[0])
                }
            }]
        }
    };
    console.log(chartOptions)

    var lineChart = new Chart(speedCanvas, {
        type: 'line',
        data: speedData,
        options: chartOptions
    });
}