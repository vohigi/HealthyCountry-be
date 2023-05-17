using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyCountry.Utilities
{
    public class ResponseErrorItem
    {
        public string Status { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public string Source { get; set; }

        private static readonly Dictionary<string, string> _statusMessages;

        static ResponseErrorItem()
        {
            _statusMessages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "PATIENT_NOT_FOUND", "There is no patient found." },
                { "WRONG_EVENT_STATE_TRANSITION", "Can not change event state: wrong state transition." },
                { "EVENT_NOT_FOUND", "There is no event found." },
                { "POLICY_CREATE_PERMISSION", "К сожалению, у вас нет прав на создание приема." }
            };
        }

        public ResponseErrorItem(){}
        public ResponseErrorItem(string status, string message = null, string source = null)
        {
            Status = status;
            Source = source;
            //TODO get some config or base
            if (message != null || _statusMessages.TryGetValue(status, out message))
                Messages.Add(message);            
        }
        public ResponseErrorItem(string status, IEnumerable<string> messages, string source = null)
        {
            Status = status;
            Source = source;
            //TODO get some config or base
            if (!messages.Any() && _statusMessages.TryGetValue(status, out var message))
                Messages.Add(message);
            else
                Messages.AddRange(messages);
        }
    }
}