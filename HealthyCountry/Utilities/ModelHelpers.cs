﻿using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HealthyCountry.Utilities
{
    public static class ModelHelpers
    {
        public static ResponseError DecorateModelState(ModelStateDictionary modelState, string status = null)
        {
            var responseError = new ResponseError();
            foreach (var v in modelState)
                if(v.Value.ValidationState == ModelValidationState.Invalid)
                    if (string.IsNullOrEmpty(v.Key))
                        responseError.AddOneError((status ?? "INVALID_INPUT_MODEL").ToUpperInvariant(), string.Join(", ", v.Value.Errors.Select(e => e.ErrorMessage)));
                    else
                    {
                        var lowerKey = ToLowerCamelcase(v.Key);
                        
                        responseError.AddOneError(
                            (status ?? "INVALID_ATTRIBUTE").ToUpperInvariant(), 
                            v.Value.Errors.Select(e => 
                                string.IsNullOrEmpty(e.ErrorMessage)
                                    ? $"The {lowerKey} field has wrong value."
                                    : e.ErrorMessage.Replace(v.Key, lowerKey)),
                            lowerKey);
                    }

            return responseError;
        }
        private static string ToLowerCamelcase(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var chain = value.Split('.');
            return string.Join('.', chain.Select(c=> c.Length == 0 ? string.Empty
                : $"{char.ToLower(c[0])}{(c.Length > 1 ? c.Substring(1) : null)}"));
        }
    }
}