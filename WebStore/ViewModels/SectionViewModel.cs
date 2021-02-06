﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public record SectionViewModel
    {
        public int Id { get; init; }
        
        public string Name { get; init; }

        public int Order { get; init; }

        public SectionViewModel Parent { get; init; }
        
        public List<SectionViewModel> ChildSections { get; } = new(); 
    }
}
