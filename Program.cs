using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization; 

namespace PiramidaFinansowa
{
    class Program
    {
        [XmlElement("piramida")]
        public static Modele.PiramidaModel piramida;

        [XmlElement("przelewy")]
        public static Modele.ListaPrzelewowModel przelewy; 

        static void Main(string[] args)
        {
            piramida = new Modele.PiramidaModel();
            try
            {
                piramida = piramida.PobierzStruktrePiramidy();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd pobrania struktury Piramidy:{ex.Message}");
            }
            if (piramida != null)
            {
                PobierzWplaty();
                WyswietlUczestnikowPiramidy();
            }
            else
            {
                Console.WriteLine($"Nie odnaleziono danych piramidy finansowej");
            }

        }

        /// <summary>
        /// Pobiera wpłaty i rodziela prowizję dla bezpośrednio przełożonych wpłacającego
        /// </summary>
        public static void PobierzWplaty()
        {
            przelewy = new Modele.ListaPrzelewowModel();
            string filename = $"{AppDomain.CurrentDomain.BaseDirectory}przelewy.xml";
            if (System.IO.File.Exists(filename))
            {
                try
                {
                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Modele.ListaPrzelewowModel));
                    using (XmlReader reader = XmlReader.Create(filename))
                    {
                        przelewy = (Modele.ListaPrzelewowModel)xmlserializer.Deserialize(reader);
                        foreach (Modele.PrzelewModel przelew in przelewy.Przelewy)
                        {
                            if (przelew.WplacajacyID == piramida.Uczestnik.ID) // jeśli wpłaca założyciel dodaje tą kwotę do swojej prowizji
                            {
                                piramida.Uczestnik.Prowizja += przelew.Kwota; 
                            }
                            else
                            {
                                List<Modele.UczestnikModel> bezposrednioPrzelozeni = piramida.PobierzBezposrednioPrzelozonychWplacajacego(przelew.WplacajacyID, piramida.Uczestnik);
                                piramida.RodzielProwizje(przelew.Kwota, bezposrednioPrzelozeni);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PobierzWplaty: {ex.Message}");
                }
            }
        }
                 


        /// <summary>
        /// Wyświetlenie listy uczestników piramidy
        /// </summary>
        public static void WyswietlUczestnikowPiramidy()
        {
            try
            {
                ICollection<Modele.UczestnikModel> Uczestnicy = piramida.PobierzUczestnikow();
                var OrderedList = Uczestnicy.OrderBy(x => x.ID).ToList();

                foreach (Modele.UczestnikModel uczestnik in OrderedList)
                {
                    Console.WriteLine($"{uczestnik.ID} {uczestnik.Poziom} {uczestnik.SubordinatesCount()} {uczestnik.Prowizja}");
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WyswietlUczestnikowPiramidy {ex.Message}");
            }
        }

    }
}
