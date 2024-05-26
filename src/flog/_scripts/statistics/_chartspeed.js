import $ from 'jquery';
import Chart from 'chart.js/auto'

export function generate() {
    $.getJSON("/assets/api/line-groundspeed.json", function (data) {
        const chartObj = document.getElementById("chart-speed");
        const config = {
            type: 'bar',
            data: {
                datasets: [{
                    label: "Maximum Ground Speed [km/h]",
                    data: data.y,
                }],
                labels: data.x
            },
            options: {
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    x: {
                        ticks: {
                            autoSkip: false,
                            maxRotation: 90,
                            minRotation: 90,
                            color: "#fff"
                        }
                    },
                    y: {
                        ticks: {
                            color: "#fff"
                        }
                    }
                }
            }
        }
  
        new Chart(
            chartObj,
            config
        );
    });
}