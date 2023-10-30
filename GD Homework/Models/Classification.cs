using System.ComponentModel.DataAnnotations;

namespace GD_Homework.Models
{
    public enum Classification : byte
    {
        [Display(Name = "Standartinis")]
        Standard = 0,

        [Display(Name = "Prabangi")]
        Luxurious = 1,

        [Display(Name = "Su apribojimais")]
        Restrictions = 2
    }
}

