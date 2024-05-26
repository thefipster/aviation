import * as fuelChart from "./../statistics/_chartfuel.js";
import * as speedChart from "./../statistics/_chartspeed.js";

// STATISTICS
if (window.location.pathname.includes("statistics")) {
  speedChart.generate();
  fuelChart.generate();
}

