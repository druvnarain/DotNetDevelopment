namespace Core.Entities
{
    //Must use pascal convention in order for ORM framework to work properly
    //When using scaffolding, uses attributes to create columns
    public class Product
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }
}