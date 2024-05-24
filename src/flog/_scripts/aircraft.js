import './common.js'
import "leaflet"

const map = L.map("map").fitWorld();

L.tileLayer(
    "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
    {
    attribution:
        "Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
    }
).addTo(map);

$.getJSON("/assets/api/park-position.json", function (data) {

    const lat = data[0];
    const lon = data[1];
    const padding = 0.005;
    const bounds = [[lat - padding, lon - padding], [lat + padding, lon + padding]];


    map.flyToBounds(bounds);
    map.on('zoomend', function () {
        L.circleMarker(data, {
            radius: 10,
            color: "Violet",
        }).addTo(map);
    });
});