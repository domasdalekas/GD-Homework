using System.ComponentModel.DataAnnotations;

namespace GD_Homework.Helpers
{
	public static class EnumExtensions
	{
		public static string GetEnumDisplayName(this Enum value)
		{
			var enumType = value.GetType();
			var enumValue = Enum.GetName(enumType, value);

			if(enumValue == null)
			{
				throw new ArgumentNullException("Enum is null");
			}

			var enumTypeFieldValue = enumType.GetField(enumValue);

			if (enumTypeFieldValue == null)
			{
                throw new ArgumentNullException("Enum type field value is null");
            }

            var displayAttribute = enumTypeFieldValue
				.GetCustomAttributes(typeof(DisplayAttribute), false)
				.SingleOrDefault() as DisplayAttribute;

			return displayAttribute?.Name ?? value.ToString();
		}
	}
}
