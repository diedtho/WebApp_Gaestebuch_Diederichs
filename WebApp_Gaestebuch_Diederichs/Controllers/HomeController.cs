using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Gaestebuch_Diederichs.Models;
using WebApp_Gaestebuch_Diederichs.ViewModels;

namespace WebApp_Gaestebuch_Diederichs.Controllers
{
    public class HomeController : Controller
    {
        private readonly SqliteConnection conn;

        public HomeController()
        {

            // 1. Connection-String
            string connStr = "Data Source =./Database/Gaestebuch.db; ";

            // 2. SQL-Connection
            conn = new SqliteConnection(connStr);

        }

        [HttpGet]
        public IActionResult Index(GBListe listSortFilter)
        {
            // Neuer Gästebucheintrag (inklusive Liste           
            GBListe listeGaestebuch = new();
            listeGaestebuch.Filter = listSortFilter.Filter;
            listeGaestebuch.FilternSortieren();

            // 3. SQL-Command
            SqliteCommand cmdSql = new SqliteCommand("SELECT * FROM Gaestebuch;", conn);

            // 4. Verbindung öffnen
            conn.Open();

            // 5. Ergebnis des Selects lesen
            var dr = cmdSql.ExecuteReader();
            while (dr.Read())
            {
                GBEintrag gbEintrag = new GBEintrag
                {
                    Id = (int)(long)dr[0],  // "int" = 32bit, "long" = 64bit
                    Titel = dr[1].ToString(),
                    NickName = dr[2].ToString(),
                    EMail = dr[3].ToString(),
                    InstagramHashtag = dr[4].ToString(),
                    TwitterHashtag = dr[5].ToString(),
                    BewertBedienung = (int)(long)dr[6],
                    BewertBrauhaus = (int)(long)dr[7],
                    Eintrag = dr[8].ToString(),
                    Datum = DateTime.Parse(dr[9].ToString()),
                    Freischaltung = (int)(long)dr[10] == 0 ? false : true

                };
                listeGaestebuch.listeGaestebuch.Add(gbEintrag);
            }

            // 6. Verbindung schließen
            conn.Close();

            listeGaestebuch.SortierTyp = listSortFilter.SortierTyp;
            listeGaestebuch.FilternSortieren();

            return View(listeGaestebuch);
            
        }

        [HttpPost]
        public IActionResult Index(GBListe gbListe, int id)
        {
            GBListe ListeFiltSort = new();
            ListeFiltSort.listeGaestebuch = gbListe.listeGaestebuch;
            ListeFiltSort.Filter = gbListe.Filter;

            // 3. SQL-Command
            SqliteCommand cmdSql = new SqliteCommand("SELECT * FROM Gaestebuch;", conn);

            // 4. Verbindung öffnen
            conn.Open();

            // 5. Ergebnis des Selects lesen
            var dr = cmdSql.ExecuteReader();
            while (dr.Read())
            {
                GBEintrag gbEintrag = new GBEintrag
                {
                    Id = (int)(long)dr[0],  // "int" = 32bit, "long" = 64bit
                    Titel = dr[1].ToString(),
                    NickName = dr[2].ToString(),
                    EMail = dr[3].ToString(),
                    InstagramHashtag = dr[4].ToString(),
                    TwitterHashtag = dr[5].ToString(),
                    BewertBedienung = (int)(long)dr[6],
                    BewertBrauhaus = (int)(long)dr[7],
                    Eintrag = dr[8].ToString(),
                    Datum = DateTime.Parse(dr[9].ToString()),
                    Freischaltung = (int)(long)dr[10] == 0 ? false : true

                };
                ListeFiltSort.listeGaestebuch.Add(gbEintrag);
            }

            // 6. Verbindung schließen
            conn.Close();

            return View(ListeFiltSort);
        }

        [HttpGet]
        public IActionResult Insert()
        {
            InsertEintrag neuerEintrag = new();

            return View(neuerEintrag);
        }

        [HttpPost]
        public IActionResult Insert(InsertEintrag neuerEintrag)
        {

            // Asp.Net-Validierung
            if (!ModelState.IsValid)
            {
                return View(neuerEintrag);
            }

            // 3. SQL-Command (insert-Statement)

            SqliteCommand cmdSqlInsert = new SqliteCommand($"INSERT INTO Gaestebuch ('Titel','Name','EMail','InstaHashTag','TwittHashTag'," +
                $"'BewertBedienung','BewertBrauhaus','Eintrag','Datum','Freigeschaltet')" +
                $" VALUES(@titel,@name,@email,@instahashtag,@twitthashtag,@bewertbedienung,@bewertbrauhaus,@eintrag,@datum,@freigeschaltet);", conn);
            cmdSqlInsert.Parameters.AddWithValue("@titel", neuerEintrag.Titel);
            cmdSqlInsert.Parameters.AddWithValue("@name", neuerEintrag.NickName);
            if (neuerEintrag.EMail!=null)
            {
                cmdSqlInsert.Parameters.AddWithValue("@email", neuerEintrag.EMail);
            } else
            {
                cmdSqlInsert.Parameters.AddWithValue("@email", DBNull.Value);
            }
            if (neuerEintrag.InstagramHashtag != null)
            {
                cmdSqlInsert.Parameters.AddWithValue("@instahashtag", neuerEintrag.InstagramHashtag);
            }
            else
            {
                cmdSqlInsert.Parameters.AddWithValue("@instahashtag", DBNull.Value);
            }
            if (neuerEintrag.TwitterHashtag != null)
            {
                cmdSqlInsert.Parameters.AddWithValue("@twitthashtag", neuerEintrag.TwitterHashtag);
            }
            else
            {
                cmdSqlInsert.Parameters.AddWithValue("@twitthashtag", DBNull.Value);
            }            
            cmdSqlInsert.Parameters.AddWithValue("@bewertbedienung", neuerEintrag.BewertBedienung);
            cmdSqlInsert.Parameters.AddWithValue("@bewertbrauhaus", neuerEintrag.BewertBrauhaus);
            cmdSqlInsert.Parameters.AddWithValue("@eintrag", neuerEintrag.Eintrag);
            cmdSqlInsert.Parameters.AddWithValue("@datum", DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            cmdSqlInsert.Parameters.AddWithValue("@freigeschaltet", 0);

            // 4. Verbindung öffnen
            conn.Open();

            // 5. Ergebnis des Selects lesen
            int ok = cmdSqlInsert.ExecuteNonQuery();
            if (ok != 1) { }

            // 6. Verbindung schließen
            conn.Close();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Admin(int Id)
        {
            // Neuer Gästebucheintrag (inklusive Liste           
            GBListe listeGaestebuch = new();

            // 3. SQL-Command
            SqliteCommand cmdSql = new SqliteCommand("SELECT * FROM Gaestebuch;", conn);

            // 4. Verbindung öffnen
            conn.Open();

            // 5. Ergebnis des Selects lesen
            var dr = cmdSql.ExecuteReader();
            while (dr.Read())
            {
                GBEintrag gbEintrag = new GBEintrag
                {
                    Id = (int)(long)dr[0],  // "int" = 32bit, "long" = 64bit
                    Titel = dr[1].ToString(),
                    NickName = dr[2].ToString(),
                    EMail = dr[3].ToString(),
                    InstagramHashtag = dr[4].ToString(),
                    TwitterHashtag = dr[5].ToString(),
                    BewertBedienung = (int)(long)dr[6],
                    BewertBrauhaus = (int)(long)dr[7],
                    Eintrag = dr[8].ToString(),
                    Datum = DateTime.Parse(dr[9].ToString()),
                    Freischaltung = (int)(long)dr[10] == 0 ? false : true

                };
                listeGaestebuch.listeGaestebuch.Add(gbEintrag);
            }

            // 6. Verbindung schließen
            conn.Close();            
            
            return View(listeGaestebuch);
        }

        [HttpPost]
        public IActionResult Admin(GBListe adminListe)
        {


            
            return View();
        }


    }
}
