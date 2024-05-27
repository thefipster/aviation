import * as fuelChart from "../statistics/_chartfuel.js";
import * as speedChart from "../statistics/_chartspeed.js";

if (window.location.pathname.includes("statistics")) {
  speedChart.generate();
  fuelChart.generate();
}
