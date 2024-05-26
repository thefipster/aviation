import { addPopups, getDefaultView, initMap } from "./../components/_map.js";
import {
  createAirportPointLayer,
  createLineLayer,
} from "./_layers.js";

import $ from "jquery";

$(function () {
  if (window.location.pathname.includes("worldmap")) {
    const container = document.getElementById("popup");
    const content = document.getElementById("popup-content");

    Promise.all([
      fetch("/assets/api/track-flown.json"),
      fetch("/assets/api/airports-planned.json"),
    ])
      .then((responses) =>
        Promise.all(responses.map((response) => response.json()))
      )
      .then((data) => {
        const view = getDefaultView();
        const map = initMap("map");

        const trackLayer = createLineLayer(data[0]);
        map.addLayer(trackLayer);

        const airportLayer = createAirportPointLayer(data[1]);
        map.addLayer(airportLayer);

        map.setView(view);
        addPopups(map, container, content);
      });
  }
});
