import { Feature } from "ol";
import { Point } from "ol/geom";
import {Vector as VectorSource} from "ol/source";
import { fromLonLat } from "ol/proj";
import VectorLayer from "ol/layer/Vector";

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
