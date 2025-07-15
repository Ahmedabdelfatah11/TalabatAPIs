using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams) 
            :base
             (p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
            (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value))
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
                    
            }
            else
                AddOrderBy(P => P.Name);
            ApplyPagination((specParams.PageIndex - 1 ) * specParams.PageSize, specParams.PageSize);
        }
        public ProductWithBrandAndCategorySpecifications(int id):base(P=>P.Id==id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
