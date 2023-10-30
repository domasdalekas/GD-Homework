using GD_Homework.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace GD_Homework.Services
{
	public class WebScrapingService : IWebScrapingService
	{
		private const string mainUrl = "https://www.geradovana.lt/";

        public WebScrapingService()
		{

		}

		public HtmlDocument GetDocument(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlDocument document = web.Load(url);

			return document;
		}

		/// <summary>
		/// Scrapes categories 
		/// </summary>
		/// <returns></returns>
		public List<Category> GetCategories()
		{
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
					var subcategoryList = new List<Subcategory>(subcategories.Count);
					foreach (var subcategory in subcategories) {
						string subcategoryLink = subcategory.GetAttributeValue("href", "");
						string subcategoryName = subcategory.InnerText.Trim();
						var subcategoryModel = new Subcategory(subcategoryName, subcategoryLink);
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
			var pages = GetSubcategoryPageCount(subcategoryLink);

			for (int i = 1; i <= pages; i++) {
				var urlWithPage = subcategoryLink + $"?page={i}";
                HtmlDocument document = GetDocument(urlWithPage);
				var nodesToGetStandardString = "//div[contains(concat(' ', @class, ' '), 'product ')]";
				var allNodes = GetRequiredNodes(nodesToGetStandardString, document);
                foreach (var node in allNodes.Where(n => IsNodeAProduct(n)))
				{
					var productPriceString = node.SelectSingleNode(".//span[@class='productprice']").InnerText;
                    var productPrice = RemoveEuroSymbolAndConvertToDecimal(productPriceString);
					var nodeProductType = node.SelectSingleNode(nodesToGetStandardString);
					var nodeProductTypeString = node.Attributes.Single(att => att.Value.Contains("product")).Value;
					Classification productClassification;
					switch(nodeProductTypeString)
					{
						case "product ":
							productClassification = Classification.Standard;
							break;
						case "product gift-premium":
							productClassification = Classification.Luxurious;
							break;
						case "product gift-forYourself":
							productClassification = Classification.Restrictions;
							break;
						default:
							throw new ArgumentNullException($"Product classification {nodeProductTypeString} not recognized");
					}
						
					products.Add(new Product(productPrice, productClassification));
				}
			}

			return products;

		}

		private bool IsNodeAProduct(HtmlNode node)
		{
            var productTitleNode = node.SelectSingleNode(".//span[@class='productname']");
            if (productTitleNode == null || productTitleNode.InnerText.Contains("Dovanų kortelė"))
            {
                return false;
            }
            var productPriceNode = node.SelectSingleNode(".//span[@class='productprice']");
            if (productPriceNode == null)
            {
                return false;
            }
            var productPriceString = productPriceNode.InnerText;
            if (productPriceString == null)
            {
                return false;
            }

			return true;
        }

		public HtmlNodeCollection GetRequiredNodes(string nodesToGet, HtmlDocument document)
		{
			return document.DocumentNode.SelectNodes(nodesToGet);
		}

		public int GetSubcategoryPageCount(string url)
		{
			var pages = 1;
			var document = GetDocument(url);

			var pagesNode = document.DocumentNode.SelectSingleNode("//select[@id='PageList']");

			if (pagesNode == null) {
				return pages;
			}

			var pagesString = pagesNode.GetAttributeValue("total-pages", "");

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
