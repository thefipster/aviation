import {
  addPopups,
  getDefaultView,
  initMap,
  createWorldmapTrackLayer,
  createWorldmapAirportLayer,
  focusLayer,
} from "../components/_map.js";

if (window.location.pathname.includes("worldmap")) {
  const container = document.getElementById("popup");
  const content = document.getElementById("popup-content");

  Promise.all([
    fetch("/assets/api/track-flown.json"),
    fetch("/assets/api/airports-planned.json"),
  ])
    .then((responses) =>
      Promise.all(responses.map((response) => response.json()))
    )
    .then((data) => {
      const view = getDefaultView();
      const map = initMap("map");

      const trackLayer = createWorldmapTrackLayer(data[0]);
      map.addLayer(trackLayer);

      const airportLayer = createWorldmapAirportLayer(data[1]);
      map.addLayer(airportLayer);

      map.setView(view);
      addPopups(map, container, content);
      focusLayer(map, trackLayer);
    });
}
