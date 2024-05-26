import { fromLonLat } from "ol/proj";
import {easeIn, easeOut} from 'ol/easing';

export function flyTo(view, latLon, duration, done) {
    const location = fromLonLat([latLon[1], latLon[0]])
    const zoom = view.getZoom();
    let parts = 2;
    let called = false;
    function callback(complete) {
      --parts;
      if (called) {
        return;
      }
      if (parts === 0 || !complete) {
        called = true;
        done(complete);
      }
    }
    view.animate(
      {
        center: location,
        duration: duration / 2,
        easing: easeOut
      },
      callback,
    );
    view.animate(
      {
        zoom: 15,
        duration: duration,
        easing: easeOut
      },
      callback,
    );
  }