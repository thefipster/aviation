import Chart from '../../node_modules/chart.js/auto/auto.js'
export function generate() {
    $.getJSON("/assets/api/line-milage.json", function (data) {
        const chartObj = document.getElementById("chart-fuel");
        const config = {
            type: 'bar',
            data: {
                datasets: [{
                    label: "Jet A1 Fuel Consumption [kg/100km]",
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