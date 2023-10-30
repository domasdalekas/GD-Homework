namespace GD_Homework.Models
{
	public class Product
	{
		public decimal Price { get; }

		public Classification Classification { get; }

		public Product(decimal price, Classification classification) {
			Price = price;
			Classification = classification;
		}
	}
}
