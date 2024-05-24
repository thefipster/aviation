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
