using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public BrandsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandsViewModel> GetBrands() =>

            _productData.GetBrands()
                .OrderBy(brand => brand.Order)
                .Select(brand => new BrandsViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    ProductCount = brand.Products.Count()
                });

    }
}
