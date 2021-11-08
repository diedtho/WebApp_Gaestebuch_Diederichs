using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_Gaestebuch_Diederichs.ViewModels
{
    public class InsertEintrag
    {
        [Required]
        [Display(Name = "Titel")]
        public string Titel { get; set; }

        [Required]
        [Display(Name = "Eintrag")]
        public string Eintrag { get; set; }

        [Required(ErrorMessage = "Name wird benötigt")]
        [Display(Name = "Name (Nick)")]
        public string NickName { get; set; }

        [RegularExpression(".*@.*[.].*", ErrorMessage = "Ungültige E-Mail-Adresse")]
        public string EMail { get; set; }

        [RegularExpression("#.*", ErrorMessage = "Ungültiger Hashtag (muss mit # beginnen")]
        public string InstagramHashtag { get; set; }

        [RegularExpression("#.*", ErrorMessage = "Ungültiger Hashtag (muss mit # beginnen")]
        public string TwitterHashtag { get; set; }

        [RegularExpression("[012345]", ErrorMessage = "Nur Bewertung zwischen 0 und 5 erlaubt")]
        public int BewertBedienung { get; set; }

        [RegularExpression("[012345]", ErrorMessage = "Nur Bewertung zwischen 0 und 5 erlaubt")]
        public int BewertBrauhaus { get; set; }
        public bool Freischaltung { get; set; }

    }
}
