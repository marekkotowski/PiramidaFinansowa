using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; 

namespace PiramidaFinansowa.Modele
{
    public class UczestnikModel 
    {
        [XmlAttribute("id")]
        public int ID { get; set; }
        [XmlElement("uczestnik")]
        public List<UczestnikModel> Podwladni { get; set; }
        [XmlIgnore]
        public int Prowizja { get; set; } 
        [XmlIgnore]
        public int Poziom { get; set; }

        public UczestnikModel()
        {
            Podwladni = new List<UczestnikModel>();
        }

        public UczestnikModel(int _id)
        {
            this.ID = _id;
            this.Podwladni = new List<UczestnikModel>(); 
        }

        /// <summary>
        /// Zliczenie liczby podwładnych 
        /// </summary>
        /// <returns></returns>
        public int SubordinatesCount()
        {
            int ilePodwladnych = 0;
            foreach (UczestnikModel uczestnik in Podwladni)
            {
                ilePodwladnych += 1 + uczestnik.SubordinatesCount();
            }
            return ilePodwladnych;
        }


        /// <summary>
        /// Lista bezpośrednio przełożonych do rozdzielnia prowizji
        /// </summary>
        /// <param name="_idContributor"></param>
        /// <param name="_supervisors"></param>
        /// <returns></returns>
        public bool ZnajdzBezposrednioPrzelozonych(int _wplacajacyID, List<UczestnikModel> _przelozeni, int _poziom)
        {
            _poziom++;
            this.Poziom = _poziom; 
            if (Podwladni.Any(x => x.ID == _wplacajacyID))
            {
                _przelozeni.Add(this);
                return true; 
            }
            else
            {
                foreach (UczestnikModel uczestnik in Podwladni)
                {
                    if (uczestnik.ZnajdzBezposrednioPrzelozonych(_wplacajacyID, _przelozeni, _poziom))
                    {
                        _przelozeni.Add(this);
                        return true;
                    }
                }
                return false; 
            }
        }

        /// <summary>
        /// Lista wszystkich uczestnków piramidy finansowej
        /// </summary>
        /// <param name="_level"></param>
        /// <param name="_uczestnicy"></param>
        public void PobierzUczestnikow(int _level, List<UczestnikModel> _uczestnicy)
        {
            _level++;
            foreach (UczestnikModel uczestnik in Podwladni)
            {
                uczestnik.Poziom = _level; 
                _uczestnicy.Add(uczestnik);
                uczestnik.PobierzUczestnikow(_level, _uczestnicy);
            }
        }

        /// <summary>
        /// Nalicza Prowizję od otrzymanej kwoty
        /// </summary>
        /// <param name="_kwota"></param>
        /// <param name="_dziel"></param>
        /// <returns></returns>
        public int NaliczenieProwizji(int _kwota, bool _dziel)
        {
            int amount;
            if (_dziel)
            {
                amount = (int)(_kwota / 2);
                this.Prowizja += amount;
                return _kwota - amount;
            }
            else
            {
                this.Prowizja += _kwota;
                return 0;
            }
        }

    }
}
