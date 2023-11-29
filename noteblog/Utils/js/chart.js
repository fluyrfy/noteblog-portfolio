
$(function () {
    $("#date").datepicker()
    const ctx = document.getElementById('myChart');
    const labels = ['一月份', '二月份', '三月份', '四月份', '五月份', '六月份', '七月份'];
    const data = {
        labels: labels,
        datasets: [{
            label: 'Number of New Articles',
            data: [65, 59, 80, 81, 56, 55, 40],
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    };
    const config = {
        type: 'line',
        data: data,
    };
    const myChart = new Chart(ctx, config);
});