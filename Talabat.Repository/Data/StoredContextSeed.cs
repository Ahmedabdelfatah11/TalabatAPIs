using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoredContextSeed
    {
        public static async Task SeedAsync(StoreContext _dbcontext)
        {

            if (_dbcontext.ProductBrands.Count() == 0)
            {
                var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbcontext.Set<ProductBrand>().Add(brand);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            if (_dbcontext.ProductCategories.Count() == 0)
            {
                var categoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var categorires = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);

                if (categorires?.Count() > 0)
                {
                    foreach (var category in categorires)
                    {
                        _dbcontext.Set<ProductCategory>().Add(category);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            if (_dbcontext.Products.Count() == 0)
            {
                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        _dbcontext.Set<Product>().Add(product);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }



        }   
    }
}
