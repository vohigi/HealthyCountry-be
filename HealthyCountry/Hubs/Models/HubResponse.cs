using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using HealthyCountry.Utilities;

namespace HealthyCountry.Hubs.Models;

public class HubResponse<TResult>
{
    public int Code { get; set; }
    public bool IsSuccess { get; set; }
    public ResponseData<TResult> Result { get; set; }
    
    public ResponseError ErrorObject { get; set; }

    public HubResponse()
    {
        
    }
    public HubResponse(bool isSuccess, int code = 204)
    {
        Code = code;
        IsSuccess = isSuccess;
        Result = new ResponseData<TResult>();
    }
    
    public HubResponse(TResult result, int code = 200)
    {
        Code = code;
        IsSuccess = true;
        Result = new ResponseData<TResult>(result);
    }
    
    
    public HubResponse(TResult result, bool hasNext, int? limit = null, int? skip = null, int code = 200)
    {
        Code = code;
        IsSuccess = true;
        Result = new ResponseData<TResult>(result)
        {
            Meta = new Dictionary<string, object>()
            {
                {"hasNext", hasNext},
                {"limit", limit},
                {"skip", skip}
            }
        };
    }
    
    public HubResponse(ServiceResponse<ValidationResult> serviceResponse)
    {
        Code = serviceResponse.Status.ToHttpStatusCode();
        IsSuccess = false;
        ErrorObject = new ResponseError();
        foreach (var error in serviceResponse.Errors.Errors.GroupBy(x=>x.PropertyName))
        {
            ErrorObject.AddOneError(serviceResponse.Status.GetDescription().ToUpperInvariant(),
                error.Select(s => s.ErrorMessage),
                error.Key?.ToLowerCamelcase());
        }
    }
}