using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Validation
{
    public class ModelValidation
    {
        public JObject ValidateModel(string corelationid, string source, string description)
        {
            dynamic errorObj = new JObject();
            errorObj.corelationid = corelationid;
            errorObj.errors = new JArray() as dynamic;
            dynamic error = new JObject();
            error.source = source;
            error.description = description;
            errorObj.errors.Add(error);
            return errorObj;
        }
    }
}
