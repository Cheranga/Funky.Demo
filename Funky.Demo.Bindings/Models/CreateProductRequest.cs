using System;

namespace Funky.Demo.Models
{
    public class CreateProductRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Code { get; set; }
        public string Name { get; set; }
    }
}