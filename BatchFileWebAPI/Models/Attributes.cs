using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Models
{
    public class Attributes
    {
        [Key]
        [Newtonsoft.Json.JsonIgnore]
        public int AttributePkID { get; set; }
        
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("value")]
        public string Value { get; set; }
        [Required]
        [Newtonsoft.Json.JsonIgnore]
        public Guid BatchId { get; set; }
        [ForeignKey("BatchId")]
        [Newtonsoft.Json.JsonIgnore]
        public virtual BatchFile BatchFile { get; set; }
    }
}
