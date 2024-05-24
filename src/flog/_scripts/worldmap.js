import "./common.js";
import "leaflet";

const map = L.map("map").fitWorld();

L.tileLayer(
  "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
  {
    attribution:
      "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
  }
).addTo(map);

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
