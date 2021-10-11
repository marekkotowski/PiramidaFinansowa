using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PiramidaFinansowa.Modele
{
    [XmlRoot(ElementName ="piramida", IsNullable =false)]
    public class PiramidaModel
    {
        [XmlElement ("uczestnik")]
        public Modele.UczestnikModel Uczestnik;

        public PiramidaModel()
        { 
        
        }

        public PiramidaModel(UczestnikModel _promoter)
        {
            this.Uczestnik = _promoter; 
        }

        /// <summary>
        /// Lista uczestnków piramidy
        /// </summary>
        /// <returns></returns>
        public List<UczestnikModel> PobierzUczestnikow()
        {
            int level = 0;
            List<UczestnikModel> Participants = new List<UczestnikModel>();
            Participants.Add(this.Uczestnik);
            this.Uczestnik.PobierzUczestnikow(level, Participants);
            return Participants; 
        }

        /// <summary>
        /// Zwraca listę bezpośrednio przełożonych wpłacającego
        /// </summary>
        /// <param name="_wplacajacyID"></param>
        /// <param name="_zalozyciel"></param>
        /// <returns></returns>
        public List<Modele.UczestnikModel> PobierzBezposrednioPrzelozonychWplacajacego(int _wplacajacyID, Modele.UczestnikModel _zalozyciel)
        {
            int poziom = 0;
            List<Modele.UczestnikModel> bezposrednioPrzelozeni = new List<Modele.UczestnikModel>();
            _zalozyciel.ZnajdzBezposrednioPrzelozonych(_wplacajacyID, bezposrednioPrzelozeni, poziom);
            return bezposrednioPrzelozeni.OrderBy(x => x.Poziom).ToList();
        }


        /// <summary>
        /// Rodziela prowizję 
        /// Założyciel otrzymuje polowe wpłaconej kwoty i przekazuje resztę do podzialu
        /// na kolejnych użytkownikow
        /// </summary>
        /// <param name="_kwota"></param>
        public void RodzielProwizje(int _kwota, List<UczestnikModel> _bezposrednioprzelozeni)
        {
            foreach (Modele.UczestnikModel przelozony in _bezposrednioprzelozeni)
            {
                if (przelozony == _bezposrednioprzelozeni.Last())
                {
                    przelozony.NaliczenieProwizji(_kwota, false);
                }
                else _kwota = przelozony.NaliczenieProwizji(_kwota, true);
            }
        }

        /// <summary>
        /// Serializacja piramdy do XML
        /// </summary>
        public void Serialization()
        {
            string filename = $"{AppDomain.CurrentDomain.BaseDirectory}piramida.xml";
            XmlSerializerNamespaces emptynamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            XmlSerializer serializer = new XmlSerializer(typeof(PiramidaModel));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            using (XmlWriter xmlWriter = XmlWriter.Create(filename, settings))
            {
                serializer.Serialize(xmlWriter, this, emptynamespaces);
            }
        }

        /// <summary>
        /// Pobranie stuktury piramidy
        /// </summary>
        /// <returns></returns>
        public PiramidaModel PobierzStruktrePiramidy()
        {
            PiramidaModel piramida;
            string filename = $"{AppDomain.CurrentDomain.BaseDirectory}piramida.xml";
            XmlSerializer xmlserializer = new XmlSerializer(typeof(Modele.PiramidaModel));
            if (System.IO.File.Exists(filename))
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    piramida = (Modele.PiramidaModel)xmlserializer.Deserialize(reader);
                    return piramida;
                }
            }
            return null; 
        }

    }
}
