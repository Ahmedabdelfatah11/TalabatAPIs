using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductsWithFilterationForCountSpecifications :BaseSpecifications<Product>
    {
        public ProductsWithFilterationForCountSpecifications(ProductSpecParams specParams)
            :base
            (p=>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search))&&
            (!specParams.BrandId.HasValue || p.BrandId==specParams.BrandId.Value)&&
            (!specParams.CategoryId.HasValue || p.CategoryId==specParams.CategoryId.Value))
        {
            
        }
    }
}
