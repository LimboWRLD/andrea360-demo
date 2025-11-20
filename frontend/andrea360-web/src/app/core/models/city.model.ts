import { Country } from "./country.model";

export interface City {
    id: string,
    name: string,
    country: Country
}