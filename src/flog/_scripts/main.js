//import "./utils/util.js";
import "./components/_tiles.js";
import "./components/_banner.js";
import "./components/_menu.js";

import "leaflet";
import "@fancyapps/fancybox";

import * as speedChart from "./statistics/speed-chart.js";
import * as fuelChart from "./statistics/fuel-chart.js";

const mapId = "map";

function initMap() {
  var mapObj = document.getElementById(mapId);
  if (mapObj) {
    const map = L.map(mapId).fitWorld();
    L.tileLayer(
      "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
      {
        attribution:
          "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
      }
    ).addTo(map);

    return map;
  }
  return undefined;
}

const map = initMap();

// MAP OBJECT GENERAL

// WORLDMAP
if (window.location.pathname.includes("worldmap")) {
  // Track
  $.getJSON("/assets/api/track-flown.json", function (data) {
    $.each(data, function (key, val) {
      var polyline = L.polyline(val.gps, { color: "Aquamarine" })
        .addTo(map)
        .bindPopup('<a href="' + val.uri + '">' + val.flt + "</a>");
    });
  });

  // Airports
  $.getJSON("/assets/api/airports-planned.json", function (data) {
    $.each(data, function (key, val) {
      L.circleMarker(val.latlon, {
        radius: 5,
        color: "Orchid",
      })
        .addTo(map)
        .bindPopup(val.name);
    });
  });
}

// STATISTICS
if (window.location.pathname.includes("statistics")) {
  speedChart.generate();
  fuelChart.generate();
}

// POST
if (window.location.pathname.includes("flights")) {
  const flightPath = window.location.href.split("/").pop();
  const flight = flightPath.slice(0, flightPath.indexOf("."));
  $.getJSON("/assets/api/flights/" + flight + "-gps.json", function (data) {
    // Track
    var polyline = L.polyline(data.trk, { color: "DodgerBlue" });

    map.flyToBounds(polyline.getBounds());
    map.on("zoomend", function () {
      // Events
      $.each(data.e, function (key, val) {
        L.circleMarker(val.latlon, {
          radius: 5,
          color: "Orange",
        })
          .addTo(map)
          .bindPopup(val.name);
      });

      // Screenshots
      $.each(data.img, function (key, val) {
        L.circleMarker(val.latlon, {
          radius: 5,
          color: "Violet",
        })
          .addTo(map)
          .bindPopup(
            '<a data-src="' +
              val.uri +
              '" data-fancybox="gallery" data-caption="' +
              val.name +
              '">' +
              val.name +
              "</a>"
          );
      });

      // Waypoints
      $.each(data.wp, function (key, val) {
        L.circleMarker(val.latlon, {
          radius: 5,
          color: "MediumSeaGreen",
        })
          .addTo(map)
          .bindPopup(val.name);
      });

      polyline.addTo(map);
    });
  });
}

// AIRPORT
if (window.location.pathname.includes("airportmap")) {
  $.getJSON("/assets/api/airports-all.json", function (data) {
    $.each(data, function (key, val) {
      let color = "";
      switch (val.t) {
        case 0:
          color = "MediumSeaGreen";
          break;
        case 1:
          color = "DodgerBlue";
          break;
        case 2:
          color = "SlateBlue";
          break;
        case 10:
          color = "Violet";
          break;
        case 20:
          color = "Orange";
          break;
        default:
          color = "Tomato";
      }
      L.circleMarker(val.p, {
        radius: 5,
        color: color,
      })
        .addTo(map)
        .bindPopup(val.i);
    });
  });
}

// AIRCRAFT
if (window.location.pathname.includes("aircraft")) {
  $.getJSON("/assets/api/park-position.json", function (data) {
    const lat = data[0];
    const lon = data[1];
    const padding = 0.005;
    const bounds = [
      [lat - padding, lon - padding],
      [lat + padding, lon + padding],
    ];

    map.flyToBounds(bounds);
    map.on("zoomend", function () {
      L.circleMarker(data, {
        radius: 10,
        color: "Violet",
      }).addTo(map);
    });
  });
}
