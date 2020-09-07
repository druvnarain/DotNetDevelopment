namespace Core.Entities
{
    //Must use pascal convention in order for ORM framework to work properly
    //When using scaffolding, uses attributes to create columns
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string PicUrl { get; set; }

        //Also Establishes a realationship with ProductType/Brand Tables
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
    }
}