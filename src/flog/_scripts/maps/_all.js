


  
$(function () {




  var map = null;

  if (window.location.pathname.includes("airportmap")) {
    const customLayer = getAirportPoints();
    map = initMap(view, [customLayer], overlay);
  }

  if (window.location.pathname.includes("aircraft")) {
    aircraftPosition().then((data) => {
      const customLayer = createPointLayer(data, "D-FIPS");
      map = initMap(view, [customLayer], overlay);
      flyTo(view, data, 5000, () => {
        console.log("flyby done");
      });
    });
  }

  if (window.location.pathname.includes("worldmap")) {
    getWorldmapData().then((data) => {
      var customLayer = createLineLayer(data[0]);
      var layer = createAirportPointLayer(data[1]);
      map = initMap(view, [customLayer, layer], overlay);
    });
  }
});






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

