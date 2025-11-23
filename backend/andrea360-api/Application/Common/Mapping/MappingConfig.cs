using Application.Billing.Transactions.Get;
using Application.Billing.UserServices.Get;
using Application.Catalog.Services.Get;
using Application.Locations.Addresses.Get;
using Application.Locations.Cities.Get;
using Application.Locations.Countries.Get;
using Application.Locations.Locations.Get;
using Application.Scheduling.Reservations.Get;
using Application.Scheduling.Sessions.Get;
using Domain.Billing;
using Domain.Catalog;
using Domain.Locations;
using Domain.Scheduling;
using Mapster;

namespace Application.Common.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Transaction, GetTransactionResponse>();

            config.NewConfig<UserService, GetUserServiceResponse>()
                .Map(dest => dest.ServiceName, src => src.Service.Name);

            config.NewConfig<Service, GetServiceResponse>();

            config.NewConfig<Location, GetLocationResponse>()
                    .Map(dest => dest.Address, src => src.Address);

            config.NewConfig<Country, GetCountryResponse>();

            config.NewConfig<City, GetCityResponse>()
                  .Map(dest => dest.Country, src => src.Country);

            config.NewConfig<Address, GetAddressResponse>()
                  .Map(dest => dest.City, src => src.City);

            config.NewConfig<Session, GetSessionResponse>()
                  .Map(dest => dest.LocationName, src => src.Location.Name)
                  .Map(dest => dest.ServiceName, src => src.Service.Name)
                  .Map(dest => dest.ServicePrice, src => src.Service.Price);

            config.NewConfig<Reservation, GetReservationResponse>()
                  .Map(dest => dest.UserName, src => src.User.Email);
        }
    }
}
