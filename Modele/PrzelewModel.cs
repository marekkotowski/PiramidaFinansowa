using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PiramidaFinansowa.Modele
{
    public class PrzelewModel
    {
        [XmlAttribute("od")]
        public int WplacajacyID { get; set; }
        [XmlAttribute("kwota")]
        public int Kwota { get; set; }

        public PrzelewModel()
        { 
        
        }

        public PrzelewModel(int _id, int _kwota)
        {
            this.WplacajacyID = _id;
            this.Kwota = _kwota; 
        }
    }
}
