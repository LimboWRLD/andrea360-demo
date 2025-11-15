using Application.Abstractions.Messaging;
using Domain.Catalog;

namespace Application.Catalog.Services.Create;

public class CreateServiceCommand : ICommand<Service>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
