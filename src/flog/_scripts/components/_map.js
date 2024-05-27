import { Map, View, Overlay, Feature } from "ol";
import { easeOut } from "ol/easing";
import { LineString, Point } from "ol/geom";
import TileLayer from "ol/layer/Tile";
import VectorLayer from "ol/layer/Vector";
import { fromLonLat } from "ol/proj";
import XYZ from "ol/source/XYZ";
import { Vector as VectorSource } from "ol/source";
import Stroke from "ol/style/Stroke";
import Style from "ol/style/Style";

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
        title = '<a href="' + link + '">' + title + "</a>";
      }

      content.innerHTML = title;
      overlay.setPosition(evt.coordinate);
    } else {
      overlay.setPosition(undefined);
    }
  });
}

export function createAircraftParkingLayer(latLon, title) {
  const coords = fromLonLat([latLon[1], latLon[0]]);

  const feature = new Feature({
    geometry: new Point(coords),
    title: title,
  });

  const features = [];
  features.push(feature);

  const source = new VectorSource();
  source.addFeatures(features);

  const layer = new VectorLayer({
    source: source,
  });

  return layer;
}

export function createWorldmapAirportLayer(airports) {
  const features = [];
  for (let airport of airports) {
    const coord = fromLonLat([airport.latlon[1], airport.latlon[0]]);
    const feature = new Feature({
      geometry: new Point(coord),
      title: airport.name,
    });

    features.push(feature);
  }

  const source = new VectorSource();
  source.addFeatures(features);

  const layer = new VectorLayer({
    source: source,
  });

  return layer;
}

export function createWorldmapTrackLayer(flights) {
  const features = [];
  for (let flight of flights) {
    let coords = [];
    for (let point of flight.gps) {
      coords.push(fromLonLat([point[1], point[0]]));
    }

    const feature = new Feature({
      geometry: new LineString(coords),
      title: flight.flt,
      link: flight.uri,
    });

    features.push(feature);
  }

  const source = new VectorSource();
  source.addFeatures(features);

  const layer = new VectorLayer({
    source: source,
    style: new Style({
      stroke: new Stroke({
        color: "#9bf1ff",
        width: 2,
      }),
    }),
  });

  return layer;
}

export function createFlightTrackLayer(flight) {
  let coords = [];
  for (let point of flight) {
    coords.push(fromLonLat([point[1], point[0]]));
  }

  const feature = new Feature({
    geometry: new LineString(coords),
  });

  const features = [];
  features.push(feature);

  const source = new VectorSource();
  source.addFeatures(features);

  const layer = new VectorLayer({
    source: source,
    style: new Style({
      stroke: new Stroke({
        color: "#9bf1ff",
        width: 2,
      }),
    }),
  });

  return layer;
}

export function createAirportsLayer(airports) {
  const source = new VectorSource();
  const features = [];
  for (const item of airports) {
    const coords = fromLonLat([item.p[1], item.p[0]]);
    features.push(
      new Feature({
        geometry: new Point(coords),
        title: item.i,
      })
    );
  }
  source.addFeatures(features);
  const layer = new VectorLayer({
    source: source,
  });

  return layer;
}
