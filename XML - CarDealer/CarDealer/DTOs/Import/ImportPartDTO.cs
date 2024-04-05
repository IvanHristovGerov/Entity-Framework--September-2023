using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("partId")]
    public class ImportPartDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}