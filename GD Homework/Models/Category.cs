namespace GD_Homework.Models
{
    public class Category
    {
        public string LinkToCategory { get; private set; }

        public string Name { get; private set; }

        public List<Subcategory> Subcategories { get; private set; }

        public Category(string linkToCategory, string name, List<Subcategory> subcategories)
        {
            LinkToCategory = linkToCategory;
            Name = name;
            Subcategories = subcategories;
        }
    }
}
