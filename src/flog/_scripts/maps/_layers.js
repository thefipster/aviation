import { Feature } from "ol";
import { LineString, Point } from "ol/geom";
import { Vector as VectorSource } from "ol/source";
import { fromLonLat } from "ol/proj";
import VectorLayer from "ol/layer/Vector";
import Style from "ol/style/Style";
import Stroke from "ol/style/Stroke";

export function createPointLayer(latLon, title) {
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

export function createAirportPointLayer(airports) {
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

export function createLineLayer(flights) {
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
        color: '#9bf1ff',
        width: 2,
      }),
    })
  });

  return layer;
}
