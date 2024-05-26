import $ from "jquery";
import { Map, View, Overlay } from "ol";
import TileLayer from "ol/layer/Tile";
import XYZ from "ol/source/XYZ";

import { getAirportPoints } from "../maps/_airportmap";
import { aircraftPosition } from "../maps/_aircraft";
import { createPointLayer } from "../maps/_layers";
import { flyTo } from "../maps/_flyto";

function initMap(view, layer, overlay) {
  return new Map({
    target: "map",
    overlays: [overlay],
    layers: [
      new TileLayer({
        source: new XYZ({
          attributions:
            'Tiles © <a href="https://services.arcgisonline.com/ArcGIS/' +
            'rest/services/World_Topo_Map/MapServer">ArcGIS</a>',
          url:
            "https://server.arcgisonline.com/ArcGIS/rest/services/" +
            "World_Imagery/MapServer/tile/{z}/{y}/{x}",
        }),
      }),
      layer,
    ],
    view: view,
  });
}

$(function () {
  const container = document.getElementById("popup");
  const content = document.getElementById("popup-content");

  const overlay = new Overlay({
    element: container,
    autoPan: {
      animation: {
        duration: 250,
      },
    },
  });

  const view = new View({
    center: [0, 0],
    zoom: 2,
  });

  var map = null;

  if (window.location.pathname.includes("airportmap")) {
    const customLayer = getAirportPoints();
    map = initMap(view, customLayer, overlay);
  }

  if (window.location.pathname.includes("aircraft")) {
    aircraftPosition().then((data) => {
      const customLayer = createPointLayer(data, "D-FIPS");
      map = initMap(view, customLayer, overlay);
      flyTo(view, data, 5000, () => { console.log("flyby done"); });
    });
  }
});
