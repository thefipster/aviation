export function getDefaultConfig(data, title) {
    return {
        type: "bar",
        data: {
          datasets: [
            {
              label: title,
              data: data.y,
              backgroundColor: ["rgba(255, 129, 94, 0.3)"],
              borderColor: ["rgb(255, 129, 94)"],
              borderWidth: 1,
            },
          ],
          labels: data.x,
        },
        options: {
          maintainAspectRatio: false,
          plugins: {
            legend: {
              display: false,
            },
          },
          scales: {
            x: {
              ticks: {
                autoSkip: true,
                maxRotation: 90,
                minRotation: 90,
                color: "#fff",
              },
              grid: {
                display: false,
              }
            },
            y: {
              ticks: {
                color: "#fff",
              },
              grid: {
                color: "rgba(212, 212, 255, 0.1)",
              },
            },
          },
        },
      };
}