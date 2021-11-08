using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_Gaestebuch_Diederichs.Models
{
    public class GBEintrag
    {
        public int Id { get; set; }
        public string Titel { get; set; }

        public string Eintrag { get; set; }
        public string NickName { get; set; }
        public string EMail { get; set; }

        public string InstagramHashtag { get; set; }
        public string TwitterHashtag { get; set; }

        public int BewertBedienung { get; set; }

        public int BewertBrauhaus { get; set; }
        public DateTime Datum { get; set; }
        public bool Freischaltung { get; set; }

        public List<GBEintrag> ListeGBEintraege { get; set; }
    }
}
