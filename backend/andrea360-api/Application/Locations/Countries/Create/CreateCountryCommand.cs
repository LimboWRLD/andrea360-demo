using Application.Abstractions.Messaging;
using Application.Locations.Countries.Get;

namespace Application.Locations.Countries.Create;
public sealed class CreateCountryCommand : ICommand<GetCountryResponse>
{
    public string Name { get; set; }
}

