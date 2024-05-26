export function mapWorld(map) {
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
}
