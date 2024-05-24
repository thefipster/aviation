import { join } from "path";

import nodeResolve from "@rollup/plugin-node-resolve";
import terser from "@rollup/plugin-terser"
import json from "@rollup/plugin-json"

const inputs = [
  "main.js"
];

// const inputs = [
//   "common.js",
//   "main.js",
//   "post.js",
//   "aircraft.js",
//   "airports.js",
//   "worldmap.js",
//   "statistics.js"
// ];

export default inputs.map(input => ({
    input: join("_scripts", input),
    output: {
      file: join("assets/js", input),
      format: 'iife',
      inlineDynamicImports: true
    },
    plugins: [
      nodeResolve(),
      terser(),
      json()
    ]
  }));