using System.ComponentModel.DataAnnotations;

namespace GD_Homework.Helpers
{
	public static class EnumExtensions
	{
		public static string GetEnumDisplayName(this Enum value)
		{
			var enumType = value.GetType();
			var enumValue = Enum.GetName(enumType, value);

			var displayAttribute = enumType.GetField(enumValue)
				.GetCustomAttributes(typeof(DisplayAttribute), false)
				.SingleOrDefault() as DisplayAttribute;

			return displayAttribute?.Name ?? value.ToString();
		}
	}
}
