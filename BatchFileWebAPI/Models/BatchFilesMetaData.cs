using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Models
{
    public class BatchFilesMetaData
    {
        [Key]
        [Newtonsoft.Json.JsonIgnore]
        public int BatchFilesMetaDataPkID { get; set; }
        [Required]
        [JsonProperty("filename")]
        public string FileName { get; set; }
        [Required]
        [JsonProperty("filesize")]
        public int FileSize { get; set; }
        [Required]
        [JsonProperty("mimeType")]
        public string MIMEType { get; set; }
        [Required]
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [Required]
        [Newtonsoft.Json.JsonIgnore]
        public Guid BatchId { get; set; }
        [ForeignKey("BatchId")]
        [Newtonsoft.Json.JsonIgnore]
        public virtual BatchFile BatchFile { get; set; }

    }
}

