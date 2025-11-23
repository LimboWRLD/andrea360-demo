export interface Session {
    id: string,
    startTime: string,
    endTime: string,
    locationId: string,
    locationName: string,
    serviceId: string,
    serviceName: string,
    maxCapacity: number,
    currentCapacity: number
}