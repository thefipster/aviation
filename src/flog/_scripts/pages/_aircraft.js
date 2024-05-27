import {
  addPopups,
  flyTo,
  getDefaultView,
  initMap,
  createAircraftParkingLayer,
} from "../components/_map.js";

if (window.location.pathname.includes("aircraft")) {
  const container = document.getElementById("popup");
  const content = document.getElementById("popup-content");

  Promise.all([fetch("/assets/api/park-position.json")])
    .then((responses) =>
      Promise.all(responses.map((response) => response.json()))
    )
    .then((data) => {
      const position = data[0];
      const layer = createAircraftParkingLayer(position, "D-FIPS");
      const view = getDefaultView();
      const map = initMap("map");
      map.addLayer(layer);
      map.setView(view);
      addPopups(map, container, content);
      flyTo(map, position, 5000);
    });
}
