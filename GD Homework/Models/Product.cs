using System.ComponentModel.DataAnnotations;

namespace GD_Homework.Models
{
	public class Product
	{
		public decimal Price { get; private set; }

		public _Classification Classification { get; private set; }

		public Product() { }

		public Product(decimal price, _Classification classification) {
			Price = price;
			Classification = classification;
		}
	}
	public enum _Classification : byte
	{
		[Display(Name = "Standartinis")]
		Standard = 0,

		[Display(Name = "Prabangi")]
		Luxurious = 1,

		[Display(Name = "Su apribojimais")]
		Restrictions = 2
	}
}
