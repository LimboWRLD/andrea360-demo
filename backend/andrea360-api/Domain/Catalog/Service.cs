using Domain.Common;
using System;

namespace Domain.Catalog
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
