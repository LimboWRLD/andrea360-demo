using Application.Abstractions.Messaging;
using Application.Catalog.Services.Get;
using Domain.Catalog;

namespace Application.Catalog.Services.Create;

public class CreateServiceCommand : ICommand<GetServiceResponse>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
