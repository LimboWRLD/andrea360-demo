export interface Session {
    id: string,
    startTime: Date,
    endTime: Date,
    locationId: string,
    locationName: string,
    serviceId: string,
    serviceName: string,
    maxCapacity: number,
    currentCapacity: number
}