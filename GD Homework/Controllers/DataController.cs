using GD_Homework.Helpers;
using GD_Homework.Models;
using GD_Homework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace GD_Homework.Controllers
{
    [ApiController]
    [Route("api")]
    public class DataController : ControllerBase
    {
        private readonly IWebScrapingService _scrapingService;

        public DataController(IWebScrapingService scrapingService)
        {
            _scrapingService = scrapingService;
        }

		[HttpGet]
		[Route("getCategoriesAndSubcategories")]
		public List<Category> GetCategoriesAndSubcategories()
		{
			return _scrapingService.GetCategories();
		}

		[HttpGet]
		[Route("getSubcategoryInformation")]
		public ActionResult GetData(string subcategoryUrl)
		{
			var productList = _scrapingService.GetProductsBySubcategory(subcategoryUrl);

			var result = productList
			   .GroupBy(p => p.Classification)
			   .Select(group => new {
				   Classification = group.Key.GetEnumDisplayName(),
				   Count = group.Count(),
				   AveragePrice = Math.Round(group.Average(p => p.Price), 2)
			   })
			   .ToList();

			return Ok(result);
		}
	}
}