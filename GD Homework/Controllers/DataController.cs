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
        private readonly ILogger<DataController> _logger;
        private readonly IWebScrapingService _scrapingService;

        public DataController(ILogger<DataController> logger, IWebScrapingService scrapingService)
        {
            _logger = logger;
            _scrapingService = scrapingService;
        }

		[HttpGet]
		[Route("getCategoriesAndSubcategories")]
		public List<Category> GetCategoriesAndSubcategories()
		{
			return _scrapingService.GetCategoriesAndSubcategories();
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