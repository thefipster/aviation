import u from "umbrellajs";
import Spotlight from "spotlight.js/src/js/spotlight.js";
import {
  addPopups,
  createFlightTrackLayer,
  createScreenshotLayer,
  createWaypointLayer,
  flyToLayer,
  getDefaultView,
  initMap,
} from "../components/_map.js";

let gallery = null;

document.addEventListener("DOMContentLoaded", function (e) {
  if (window.location.pathname.includes("flights")) {
    const flightPath = window.location.href.split("/").pop();
    const flight = flightPath.slice(0, flightPath.indexOf("."));
  
    Promise.all([fetch("/assets/api/flights/" + flight + "-gps.json")])
      .then((responses) =>
        Promise.all(responses.map((response) => response.json()))
      )
      .then((data) => {
        var result = data[0];
        handleMap(result);
        handleGallery(result.img);
      });
  }
});

export function openGallery(image) {
  console.log("OPEN: " + image);
  console.log(gallery);

  Spotlight.show(gallery, { index: image });
}

function handleGallery(images) {
  u(".gallery-image").on("click", function(e) {
    e.preventDefault();
    e.stopPropagation();
    var index = u(this).data("index");
    openGallery(index);
  });

  var gal = [];
  for(let image of images) {
    var img = {
      src: image.uri,
      title: image.name
    }
    gal.push(img);
  }

  gallery = gal;
}

function handleMap(result) {
  const trackLayer = createFlightTrackLayer(result.trk);
  const waypointLayer = createWaypointLayer(
    result.wp,
    "#87c5a4",
    "#87c5a499"
  );
  const eventsLayer = createWaypointLayer(result.e, "#e7b788", "#e7b78899");
  const imageLayer = createScreenshotLayer(result.img);
  const view = getDefaultView();
  const map = initMap("map");
  map.addLayer(trackLayer);
  map.addLayer(waypointLayer);
  map.addLayer(eventsLayer);
  map.addLayer(imageLayer);
  map.setView(view);
  addPopups(map);
  flyToLayer(map, trackLayer, 5000, 0.2);
}
