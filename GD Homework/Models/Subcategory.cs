namespace GD_Homework.Models
{
    public class Subcategory
    {
        public string Name { get; }

        public string LinkToSubCategory { get; }

        public Subcategory(string name, string linkToSubCategory)
        {
            Name = name;
            LinkToSubCategory = linkToSubCategory;
        }
    }
}
