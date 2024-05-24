(function ($) {
  Fancybox.bind("[data-fancybox]", {});
  Fancybox.bind('[data-fancybox="gallery"]', {});

  const element = document.getElementById("map");
  if (element) {
    var map = L.map("map").fitWorld();

    L.tileLayer(
      "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
      {
        attribution:
          "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
      }
    ).addTo(map);

    populateMap(map);
  }

  const chartObj = document.getElementById("chart-loader");
  if (chartObj) {
    populateChart();
  }
})(jQuery);
