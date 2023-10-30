using GD_Homework.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace GD_Homework.Services
{
	public interface IWebScrapingService
	{
		List<Category> GetCategories();

		List<Product> GetProductsBySubcategory(string subcategoryLink);
	}

	public class WebScrapingService : IWebScrapingService
	{
		public WebScrapingService()
		{

		}

		public HtmlDocument GetDocument(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlDocument document = web.Load(url);

			return document;
		}

		public List<Category> GetCategories()
		{
			var mainUrl = "https://www.geradovana.lt/";

			var categories = new List<Category>();

			HtmlDocument document = GetDocument(mainUrl);

			var nodesToGetString = "//div[contains(concat(' ', @class, ' '), 'mega-menu')]" +
				"//ul[contains(concat(' ', @class, ' '), 'mega-menu__categories')]" +
				"//ul[contains(concat(' ', @class, ' '), 'submenu')]";

			var nodes = GetRequiredNodes(nodesToGetString, document);

			foreach (var node in nodes) {
				var category = node.SelectSingleNode(".//a[@class='list-item list-item--arrow']");
				if (category != null) {
					string categoryLink = category.GetAttributeValue("href", "");
					string categoryName = category.SelectSingleNode(".//b").InnerText;

					var subcategories = node.SelectNodes(".//a[@class='list-item']");
					var subcategoryList = new List<Subcategory>();
					foreach (var subcategory in subcategories) {
						string subcategoryLink = subcategory.GetAttributeValue("href", "");
						string subcategoryName = subcategory.InnerText.Trim();
						var products = new List<Product>();
						var subcategoryModel = new Subcategory(subcategoryName, subcategoryLink, products);
						subcategoryList.Add(subcategoryModel);
					}
					var categoryModel = new Category(categoryLink, categoryName, subcategoryList);
					categories.Add(categoryModel);
				}
			}

			return categories;
		}

		public List<Product> GetProductsBySubcategory(string subcategoryLink)
		{
			var products = new List<Product>();
			HtmlDocument document = new HtmlDocument();
			var pages = GetSubcategoryPageCount(subcategoryLink);
			try {
				for (int i = 1; i <= pages; i++) {
					var urlWithPage = subcategoryLink + $"?page={i}";
					document = GetDocument(urlWithPage);
					var nodesToGetStandardString = "//div[contains(concat(' ', @class, ' '), 'product ')]";
					var allNodes = GetRequiredNodes(nodesToGetStandardString, document);
					var productModel = new Product();
					foreach (var node in allNodes) {
						var productTitleNode = node.SelectSingleNode(".//span[@class='productname']");
						if (productTitleNode == null || productTitleNode.InnerText.Contains("Dovanų kortelė")) {
							continue;
						}
						var productPriceNode = node.SelectSingleNode(".//span[@class='productprice']");
						if (productPriceNode == null) {
							continue;
						}
						var productPriceString = productPriceNode.InnerText;
						if (productPriceString == null) {
							continue;
						}
						var productPrice = RemoveEuroSymbolAndConvertToDecimal(productPriceString);
						if (node.Attributes[0].Value.Contains("gift-premium")) {
							productModel = new Product(productPrice, _Classification.Luxurious);
						} else if (node.Attributes[0].Value.Contains("gift-forYourself")) {
							productModel = new Product(productPrice, _Classification.Restrictions);
						} else {
							productModel = new Product(productPrice, _Classification.Standard);
						}
						products.Add(productModel);
					}
				}

				return products;
			} catch(Exception ex) {
				throw ex;
			}
		}

		public HtmlNodeCollection GetRequiredNodes(string nodesToGet, HtmlDocument document)
		{
			return document.DocumentNode.SelectNodes(nodesToGet);
		}

		public int GetSubcategoryPageCount(string url)
		{
			var pages = 1;
			var pagesString = "";
			var document = GetDocument(url);

			var pagesNode = document.DocumentNode.SelectSingleNode("//select[@id='PageList']");

			if (pagesNode == null) {
				return pages;
			}

			pagesString = pagesNode.GetAttributeValue("total-pages", "");

			if (int.TryParse(pagesString, out pages)) {
				return pages;
			}

			return pages;
		}

		public decimal RemoveEuroSymbolAndConvertToDecimal(string value)
		{
			if (!value.Contains("€")) {
				return 0;
			}

			string pattern = @"\d+,\d{2}";
			Match match = Regex.Match(value, pattern);

			if (match.Success) {
				var resultString = match.Value.Replace(",", ".");

				if (decimal.TryParse(resultString, out decimal decimalValue)) {
					return decimalValue;
				}
			}
			return 0;
		}
	}
}
