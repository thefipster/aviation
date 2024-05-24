import { join } from "path";

import nodeResolve from "@rollup/plugin-node-resolve";
import terser from "@rollup/plugin-terser"
import json from "@rollup/plugin-json"

const inputs = [
    "common.js",
    "post.js",
    "aircraft.js",
    "airports.js",
    "worldmap.js",
    "statistics.js"
];

export default inputs.map(input => ({
    input: join("_scripts", input),
    output: {
      file: join("assets/bundle", input),
      format: 'iife',
      inlineDynamicImports: true
    },
    plugins: [
      nodeResolve(),
      terser(),
      json()
    ]
  }));