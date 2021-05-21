using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Models
{
    public class BatchFile
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BatchId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset BatchPublishedDate { get; set; }
        [Required]
        [JsonProperty("businessUnit")]
        public string BusinessUnit { get; set; }

        [JsonProperty("acl")]
        public AccessControl AccessControl { get; set; }

        [JsonProperty("attributes")]
        public List<Attributes> Attributes { get; set; }

        [JsonProperty("expiryDate")]
        public DateTimeOffset ExpiryDate { get; set; }

        [JsonProperty("files")]
        public List<BatchFilesMetaData> BatchFileMetaData { get; set; }

    }
}
