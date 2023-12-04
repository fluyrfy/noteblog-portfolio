let notesChart;
let visitsChart;
let regionsChart;
let ctrChart;
const chartInfo = [
	{
		label: "Number of New notes",
		type: "line",
		element: "notesChart",
		chart: notesChart,
	},
	{
		label: "Number of New visits",
		type: "line",
		element: "visitsChart",
		chart: visitsChart,
	},
	{
		label: "Visits of Regions",
		type: "pie",
		element: "regionsChart",
		chart: regionsChart,
	},
	{
		label: "CTR of note",
		type: "pie",
		element: "ctrChart",
		chart: ctrChart,
	},
];
$(function () {
	const today = new Date();
	const firstDay = new Date(today.setMonth(today.getMonth() - 5))
		.toISOString()
		.split("T")[0];
	const lastDay = new Date().toISOString().split("T")[0];

	$("#startDate").datepicker({
		dateFormat: "yy-mm-dd",
	});
	$("#endDate").datepicker({
		dateFormat: "yy-mm-dd",
	});
	$("#startDate").val(firstDay);
	$("#endDate").val(lastDay);
	getChartsData($("#startDate").val(), $("#endDate").val());
	$(".btn-search").on("click", function () {
		getChartsData($("#startDate").val(), $("#endDate").val());
	});
});

function getChartsData(startDate, endDate) {
	$.ajax({
		type: "GET",
		url: "/api/stats/get",
		data: { startDate, endDate },
	})
		.done((res) => {
			Object.keys(res).forEach((key, index) => {
				renderChart(res[key].filter, res[key].results, chartInfo[index]);
			});
		})
		.fail((error) => console.error(error));
}

function renderChart(filter, dataSet, info) {
	if (info.chart) {
		info.chart.data.labels = filter;
		info.chart.data.datasets.forEach((dataset) => {
			dataset.data = dataSet;
		});
		info.chart.update();
		return;
	}
	const ctx = document.getElementById(info.element);
	const data = {
		labels: filter,
		datasets: [
			{
				label: info.label,
				data: dataSet,
				fill: false,
				borderColor: randomColor(),
				backgroundColor: randomColor(dataSet.length),
				tension: 0.1,
			},
		],
	};
	const config = {
		type: info.type,
		data: data,
	};
	info.chart = new Chart(ctx, config);
}

function randomColor(number = 1) {
	let rgb = [];
	for (let i = 1; i <= number; i++) {
		const randomBetween = (min, max) =>
			min + Math.floor(Math.random() * (max - min + 1));
		const r = randomBetween(0, 255);
		const g = randomBetween(0, 255);
		const b = randomBetween(0, 255);
		rgb.push(`rgb(${r},${g},${b})`);
	}
	return number === 1 ? rgb[0] : rgb;
}
