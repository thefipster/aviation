import $ from "jquery";
import { Map, View, Overlay } from "ol";
import TileLayer from "ol/layer/Tile";
import XYZ from "ol/source/XYZ";

import { getAirportPoints } from "../maps/_airportmap";

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

  var customLayer = undefined;

  if (window.location.pathname.includes("airportmap")) {
    customLayer = getAirportPoints();
  }

  if (customLayer) {
    const map = new Map({
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
        customLayer,
      ],
      view: new View({
        center: [0, 0],
        zoom: 2,
      }),
    });

    map.setView(
      new View({
        center: [0, 0],
        zoom: 2,
      })
    );

    map.on("click", function (evt) {
      const feature = map.forEachFeatureAtPixel(evt.pixel, function (feature) {
        return feature;
      });
      if (feature) {
        const coordinates = feature.getGeometry().getCoordinates();
        const icao = feature.get("title");
        content.innerHTML = icao;
        overlay.setPosition(coordinates);
      } else {
        overlay.setPosition(undefined);
      }
    });
  }
});
