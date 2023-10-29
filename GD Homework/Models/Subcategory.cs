namespace GD_Homework.Models
{
    public class Subcategory
    {
        public string Name { get; private set; }

        public string LinkToSubCategory { get; private set; }

        public List<Product> Products { get; private set; }

        public Subcategory(string name, string linkToSubCategory, List<Product> products)
        {
            Name = name;
            LinkToSubCategory = linkToSubCategory;
            Products = products;
        }
    }
}
