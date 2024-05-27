import { addPopups, getDefaultView, initMap, createAirportsLayer } from "../components/_map.js";
import $ from "jquery";

$(function () {
  if (window.location.pathname.includes("airportmap")) {
    const container = document.getElementById("popup");
    const content = document.getElementById("popup-content");

    Promise.all([fetch("/assets/api/airports-all.json")])
      .then((responses) => Promise.all(
        responses.map((response) => response.json())
      ))
      .then((data) => {
        const layer = createAirportsLayer(data[0]);
        const view = getDefaultView();
        const map = initMap("map");
        map.addLayer(layer);
        map.setView(view);
        addPopups(map, container, content);
      });
  }
});