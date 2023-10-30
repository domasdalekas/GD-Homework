namespace GD_Homework.Models
{
    public class Category
    {
        public string LinkToCategory { get; }

        public string Name { get; }

        public List<Subcategory> Subcategories { get; }

        public Category(string linkToCategory, string name, List<Subcategory> subcategories)
        {
            LinkToCategory = linkToCategory;
            Name = name;
            Subcategories = subcategories;
        }
    }
}
