using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            IEnumerable<Product> query = TestData.Products;

            if (filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);

            if (filter?.BrandId is { } brand_id)
                query = query.Where(product => product.BrandId == brand_id);

            return query;
        }

        public Product GetProductById(int id) => TestData.Products.FirstOrDefault(product => product.Id == id);
    }
}
