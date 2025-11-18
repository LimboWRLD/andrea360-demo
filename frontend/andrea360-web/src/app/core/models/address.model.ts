import { City } from "./city.model";

export interface Address {
    id: string,
    street: string,
    number: string,
    city: City
}