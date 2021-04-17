﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product product) => product is null ? null : new ProductViewModel 
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand?.Name,
        };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products) => products.Select(ToView);

        public static Product FromView(this ProductViewModel product) => product is null ? null : new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand is null ? null : new Brand { Name = product.Name }
        };
    }
}
