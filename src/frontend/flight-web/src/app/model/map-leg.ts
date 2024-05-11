import { Point } from "./point";

export interface MapLeg {
    no: number;
    isFlown: boolean;
    departure: Point;
    arrival: Point;
}
