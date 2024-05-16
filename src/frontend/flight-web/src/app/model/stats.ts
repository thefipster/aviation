import { Landing } from "./landing"

export interface Stats {
    fuelRamp: number
    fuelShutdown: number
    fuelUsed: number
    departure: string
    arrival: string
    route: string
    departureAt: number
    arrivalAt: number
    flightTime: number
    airacCycle: string
    dispatchAt: number
    altitude: number
    windComponent: number
    greatCircleDistance: number
    routeDistance: number
    fuelPlanned: number
    prepTime: number
    fuelDelta: number
    landing: Landing
    tasPlanned: number
    fileType: string
}