using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        //Used for getProducts
        public ProductsWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);

        }
        //Used for getProduct, base(..) replaces Expression<Func<T, bool>> criteria in BasesPecification Constructor
        public ProductsWithTypesAndBrandsSpecification(int id) 
            : base(x => x.Id == id) 
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}