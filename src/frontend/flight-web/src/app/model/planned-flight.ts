import { Airport } from "./airport"

export interface PlannedFlight {
    leg: number
    departure: Airport
    arrival: Airport
  }