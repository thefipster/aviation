import { Feature } from "ol";
import { Point } from "ol/geom";
import {Vector as VectorSource} from "ol/source";
import { fromLonLat } from "ol/proj";
import VectorLayer from "ol/layer/Vector";

export function getAirportPoints() {
  const source = new VectorSource();
  const client = new XMLHttpRequest();
  client.open("GET", "/assets/api/airports-all.json");
  client.onload = function () {
    const response = client.responseText;
    const data = JSON.parse(response);
    const features = [];
    for (const item of data) {
      const coords = fromLonLat([item.p[1], item.p[0]]);
      features.push(
        new Feature({
          geometry: new Point(coords),
          title: item.i
        })
      );
    }
    source.addFeatures(features);
  };

  client.send();

  const layer = new VectorLayer({
    source: source
  });

//   const airports = new WebGLPointsLayer({
//     style: {
//       // by using an exponential interpolation with a base of 2 we can make it so that circles will have a fixed size
//       // in world coordinates between zoom level 5 and 15
//       "circle-radius": 3,
//       "circle-stroke-color": [155, 241, 255],
//       "circle-stroke-width": 2,
//       "circle-displacement": [0, 0],
//       "circle-opacity": 0.95,
//     },
//   });


  return layer;
}
