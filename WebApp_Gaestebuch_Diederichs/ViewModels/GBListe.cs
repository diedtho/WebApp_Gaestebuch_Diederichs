using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Gaestebuch_Diederichs.Models;

namespace WebApp_Gaestebuch_Diederichs.ViewModels
{
    public class GBListe
    {
        public List<GBEintrag> listeGaestebuch = new();

        [Display(Name = "Sortiertyp")]
        public string SortierTyp { get; set; }

        [Display(Name = "Titelfilter")]
        public string Filter { get; set; }


        public void FilternSortieren()
        {
            if (SortierTyp == "DateTimeDesc") { listeGaestebuch = listeGaestebuch.OrderByDescending(o => o.Datum).ToList(); }
            if (SortierTyp == "BewertBrauhaus") { listeGaestebuch = listeGaestebuch.OrderBy(o => o.BewertBrauhaus).ToList(); }
            
        }

    }
}
