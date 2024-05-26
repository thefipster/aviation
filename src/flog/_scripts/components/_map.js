import { Map, View, Overlay } from "ol";
import TileLayer from "ol/layer/Tile";
import XYZ from "ol/source/XYZ";
import { fromLonLat } from "ol/proj";
import { easeOut } from "ol/easing";

export function initMap(mapId) {
  const tileLayer = new TileLayer({
    source: new XYZ({
      attributions:
        'Tiles © <a href="https://services.arcgisonline.com/ArcGIS/' +
        'rest/services/World_Topo_Map/MapServer">ArcGIS</a>',
      url:
        "https://server.arcgisonline.com/ArcGIS/rest/services/" +
        "World_Imagery/MapServer/tile/{z}/{y}/{x}",
    }),
  });

  const map = new Map({
    target: mapId,
    layers: [tileLayer],
  });

  return map;
}

let parts = 2;
let called = false;
function flytoCallback(complete) {
  --parts;
  if (called) {
    return;
  }
  if (parts === 0 || !complete) {
    called = true;
  }
}

export function flyTo(view, latLon, duration) {
  const location = fromLonLat([latLon[1], latLon[0]]);
  view.animate(
    {
      center: location,
      duration: duration / 2,
      easing: easeOut,
    },
    flytoCallback
  );
  view.animate(
    {
      zoom: 15,
      duration: duration,
      easing: easeOut,
    },
    flytoCallback
  );
}

export function getDefaultView() {
  return new View({
    center: [0, 0],
    zoom: 2,
  });
}

export function addPopups(map, container, content) {
  const overlay = new Overlay({
    element: container,
    autoPan: {
      animation: {
        duration: 250,
      },
    },
  });

  map.addOverlay(overlay);

  map.on("click", function (evt) {
    console.log(evt);
    const feature = map.forEachFeatureAtPixel(evt.pixel, function (feature) {
      return feature;
    });
    if (feature) {
      let title = feature.get("title");
      const link = feature.get("link");

      if (link) {
        title = '<a href="' + link + '">' + title + '</a>';
      }

      content.innerHTML = title;
      overlay.setPosition(evt.coordinate);
    } else {
      overlay.setPosition(undefined);
    }
  });
}
