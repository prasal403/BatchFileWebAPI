using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Models
{
    public class AccessControl
    {
        [Key]
        [Newtonsoft.Json.JsonIgnore]
        public int AclPKID { get; set; }
        [JsonProperty("readUsers")]
        public IEnumerable<string> ReadUsers { get; set; }

        [JsonProperty("readGroups")]
        public IEnumerable<string> ReadGroups { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Guid BatchId { get; set; }
        [ForeignKey("BatchId")]
        [Newtonsoft.Json.JsonIgnore]
        public virtual BatchFile BatchFile { get; set; }
    }
}
