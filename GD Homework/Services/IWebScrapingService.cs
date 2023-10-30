using GD_Homework.Models;

namespace GD_Homework.Services
{
    public interface IWebScrapingService
    {
        List<Category> GetCategories();

        List<Product> GetProductsBySubcategory(string subcategoryLink);
    }
}

