using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        [JsonProperty("FullName")]
        public string FullName { get; set; }

        [Required]
        [Range(0, 2)]
        [JsonProperty("AgeGroup")]
        public string AgeGroup { get; set; }

        [Required]
        [Range(0, 1)]
        [JsonProperty("Gender")]
        public string Gender { get; set; }

        [JsonProperty("Medicines")]
        public List<int> Medicines { get; set; }
    }
}
