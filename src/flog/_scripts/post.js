import "./common.js"
import "leaflet"
import "@fancyapps/fancybox"

const map = L.map("map").fitWorld();

L.tileLayer(
    "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
    {
    attribution:
        "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
    }
).addTo(map);

const flightPath = window.location.href.split("/").pop()
const flight = flightPath.slice(0, flightPath.indexOf('.'));
$.getJSON("/assets/api/flights/" + flight + "-gps.json", function (data) {
    // Track
    var polyline = L.polyline(data.trk, { color: 'DodgerBlue' });
    
    map.flyToBounds(polyline.getBounds());
    map.on('zoomend', function() {
        // Events
        $.each(data.e, function (key, val) {
            L.circleMarker(val.latlon, {
                radius: 5,
                color: "Orange",
            }).addTo(map).bindPopup(val.name);
        });

        // Screenshots
        $.each(data.img, function (key, val) {
            L.circleMarker(val.latlon, {
                radius: 5,
                color: "Violet",
            }).addTo(map).bindPopup('<a data-src="' + val.uri + '" data-fancybox="gallery" data-caption="' + val.name + '">' + val.name + '</a>');
        });

        // Waypoints
        $.each(data.wp, function (key, val) {
            L.circleMarker(val.latlon, {
                radius: 5,
                color: "MediumSeaGreen",
            }).addTo(map).bindPopup(val.name);
        });

        polyline.addTo(map);
    });
});
