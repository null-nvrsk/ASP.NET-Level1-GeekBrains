﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter filter = null);

        Product GetProductById(int id);
    }
}
