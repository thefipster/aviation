using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Merged;

namespace TheFipster.Aviation.Modules.Merger
{
    public class FlightMerger
    {
        public Flight Merge(SimToolkitProFlight toolkit, SimBriefFlight simbrief, BlackBoxFlight blackbox)
        {
            var flight = new Flight();

            flight.Airline = simbrief.Airline;
            flight.FlightNumber = simbrief.FlightNumber;
            flight.AiracCycle = simbrief.AiracCycle;
            flight.AirDistance = simbrief.AirDistance;
            flight.GreatCircleDistance = simbrief.GreatCircleDistance;
            flight.RouteDistance = simbrief.RouteDistance;
            flight.Route = simbrief.Route;
            flight.Altitude = simbrief.Altitude;
            flight.CostIndex = simbrief.CostIndex;
            flight.DispatchedOn = simbrief.DispatchDate;
            flight.WindComponent = simbrief.WindComponent;
            flight.Passengers = simbrief.Passengers;
            flight.Departure = simbrief.Departure.Icao;
            flight.Arrival = simbrief.Arrival.Icao;
            flight.Alternate = simbrief.Alternate.Icao;

            flight.DepartedOn = long.Parse(toolkit.Logbook.ActualDep);
            flight.ArrivedOn = long.Parse(toolkit.Logbook.ActualArr);
            flight.FuelRamp = toolkit.Logbook.FuelRamp;
            flight.FuelShutdown = toolkit.Logbook.FuelShutdown;
            flight.Aircraft = toolkit.Landing.AircraftName;

            return flight;
        }
    }
}
