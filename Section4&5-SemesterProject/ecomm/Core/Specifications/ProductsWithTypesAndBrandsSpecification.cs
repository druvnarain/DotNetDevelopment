using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        //Used for getProducts
        public ProductsWithTypesAndBrandsSpecification(
            //string sort, int? brandId, int? typeId   replaced with ProductSpecParams
            ProductSpecParams productParams)
        : base(x => 
            // || reads as "or else..." so either it sets it to null or else the brandId/typeId that is passed
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            //dont want to skip first 5, so do minus 1
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch(productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc" :
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
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