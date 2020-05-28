using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HealthyCountry.Utilities
{
    public class ResponseError
    {
        [JsonIgnore]
        public ResponseErrorItem[] Errors
        {
            get => ErrorItems?.ToArray();
            set=> ErrorItems = value?.ToList();
        }
        [JsonProperty("errors")]
        internal List<ResponseErrorItem> ErrorItems { get; set; }
        internal bool HasErrors { get => ErrorItems.Count > 0; }

        public string ErrorText { get; set; }

        // constructors
        internal ResponseError()
        {
            ErrorItems = new List<ResponseErrorItem>();
        }
        internal ResponseError(ResponseErrorItem item)
        {
            ErrorItems = new List<ResponseErrorItem>();
            AddOneError(item);
        }
        internal ResponseError(string status, string message = null, string source = null)
        {
            ErrorItems = new List<ResponseErrorItem>();
            AddOneError(status, message, source);
        }
        // methods
        internal void AddOneError(ResponseErrorItem item)
        {
            ErrorItems.Add(item);
        }
        internal void AddOneError(string status, string message = null, string source = null)
        {
            AddOneError(new ResponseErrorItem(status, message, source));
        }
        internal void AddOneError(string status, IEnumerable<string> messages, string source = null)
        {
            AddOneError(new ResponseErrorItem(status, messages, source));
        }
        internal void AddManyErrors(IEnumerable<ResponseErrorItem> items)
        {
            ErrorItems.AddRange(items);
        }

        internal void AddRange(List<ResponseErrorItem> errors)
        {
            ErrorItems.AddRange(errors);
        }
    }
}