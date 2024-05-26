import * as $ from "jquery";

import { mapWorld } from "./../maps/_worldmap";

(function ($) {
  var mapObj = document.getElementById("map");
  if (mapObj) {
    const map = L.map("map").fitWorld();
    L.tileLayer(
      "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
      {
        attribution:
          "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
      }
    ).addTo(map);

    mapWorld(map);
  }
})(jQuery);
