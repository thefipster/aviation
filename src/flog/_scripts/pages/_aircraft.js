import { addPopups, flyTo, getDefaultView, initMap, createAircraftParkingLayer } from "../components/_map.js";
import $ from "jquery";

$(function () {
  if (window.location.pathname.includes("aircraft")) {
    const container = document.getElementById("popup");
    const content = document.getElementById("popup-content");

    Promise.all([fetch("/assets/api/park-position.json")])
      .then((responses) => Promise.all(
        responses.map((response) => response.json())
      ))
      .then((data) => {
        const layer = createAircraftParkingLayer(data[0], "D-FIPS");
        const view = getDefaultView();
        const map = initMap("map");
        map.addLayer(layer);
        map.setView(view);
        addPopups(map, container, content);
        flyTo(view, data[0], 5000);
      });
  }
});
