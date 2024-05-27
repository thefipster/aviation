import Spotlight from "spotlight.js";
import { addPopups, createFlightTrackLayer, flyTo, getDefaultView, initMap } from "../components/_map.js";

import $ from "jquery";
import { fromLonLat } from "ol/proj.js";

$(function () {

  if (window.location.pathname.includes("flights")) {
    const flightPath = window.location.href.split("/").pop();
    const flight = flightPath.slice(0, flightPath.indexOf("."));

    const container = document.getElementById("popup");
    const content = document.getElementById("popup-content");

    Promise.all([fetch("/assets/api/flights/" + flight + "-gps.json")])
      .then((responses) => Promise.all(
        responses.map((response) => response.json())
      ))
      .then((data) => {
        var result = data[0];

        const layer = createFlightTrackLayer(result.trk);
        const view = getDefaultView();
        const map = initMap("map");
        map.addLayer(layer);
        map.setView(view);
        // addPopups(map, container, content);
        // flyTo(view, data[0], 5000);
        var layerExtent = layer.getSource().getExtent();

        var padding = [100, 100, 100, 100];
        map.getView().fit(layerExtent, {
          size: map.getSize(),
          padding: padding,
        });
      });
  }
});
