﻿using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.ViewModels
{
    public record SectionViewModel
    {
        public int Id { get; init; }
        
        public string Name { get; init; }

        public int Order { get; init; }

        public SectionViewModel Parent { get; init; }
        
        public List<SectionViewModel> ChildSections { get; } = new();

        public int ProductsCount { get; set; }

        public int TotalProductCount => ProductsCount + ChildSections.Sum(c => c.TotalProductCount);
    }
}
