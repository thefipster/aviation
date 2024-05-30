import { join } from "path";

import cjs from '@rollup/plugin-commonjs';
import nodeResolve from "@rollup/plugin-node-resolve";
import terser from "@rollup/plugin-terser"
import json from "@rollup/plugin-json"

const inputs = [
  "main.js"
];

export default inputs.map(input => ({
    input: join("_scripts", "_" + input),
    output: {
      file: join("assets/js", input),
      format: 'iife',
      inlineDynamicImports: true
    },
    plugins: [
      nodeResolve({browser: true}),
      cjs(),
      terser(),
      json()
    ]
  }));