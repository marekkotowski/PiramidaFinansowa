using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PiramidaFinansowa.Modele
{
    [XmlRoot("przelewy")]
    public class ListaPrzelewowModel
    {
        [XmlElement("przelew")]
        public List<PrzelewModel> Przelewy;

        public ListaPrzelewowModel()
        {
            Przelewy = new List<PrzelewModel>();
        }


        public void Serialization()
        {
            XmlSerializerNamespaces emptynamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            XmlSerializer serializer = new XmlSerializer(typeof(PrzelewModel));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };
            using (XmlWriter xmlWriter = XmlWriter.Create("przelewy.xml", settings))
            {
                serializer.Serialize(xmlWriter, this, emptynamespaces);
            }
        }

        

    }
}
