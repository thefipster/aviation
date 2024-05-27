import {
  Chart,
  BarController,
  BarElement,
  LinearScale,
  CategoryScale,
} from "chart.js";
import { getDefaultConfig } from "../components/_chart";

Chart.register(BarController, BarElement, LinearScale, CategoryScale);

export function generate() {
  const fetchPromise = fetch("/assets/api/line-milage.json");
  fetchPromise
    .then((response) => {
      return response.json();
    })
    .then((data) => {
      const element = document.getElementById("chart-speed");
      const config = getDefaultConfig(data, "Maximum Ground Speed [km/h]");
      new Chart(element, config);
    });
}
