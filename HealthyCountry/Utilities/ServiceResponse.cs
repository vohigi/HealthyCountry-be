using System.ComponentModel;
using System.Net;

namespace HealthyCountry.Utilities
{
    public class ServiceResponse<TErrorRepresentation>
    {
        public bool IsSuccess { get; set; }
        public TErrorRepresentation Errors { get; set; }
        public ServiceResponseStatuses Status { get; set; }

        public ServiceResponse()
        {
            
        }
        public ServiceResponse(TErrorRepresentation errors, ServiceResponseStatuses status)
        {
            Errors = errors;
            Status = status;
        }
    }
    public class ServiceResponse<TResult, TErrorRepresentation> : ServiceResponse<TErrorRepresentation>
    {
        public TResult Result { get; set; }

        public ServiceResponse() : base()
        {
            
        }
        public ServiceResponse(TResult result)
        {
            IsSuccess = true;
            Result = result;
        }

        public ServiceResponse(TErrorRepresentation errors, ServiceResponseStatuses status) : base(errors, status)
        {
            
        }
    }

    public static class ServiceResponseStatusesExtensions
    {
        public static int ToHttpStatusCode(this ServiceResponseStatuses status)
            => (int)_convertToHttpStatusCode(status);
        private static HttpStatusCode _convertToHttpStatusCode(ServiceResponseStatuses status)
        {
            switch (status)
            {
                case ServiceResponseStatuses.Success:
                    return HttpStatusCode.OK;
                case ServiceResponseStatuses.Conflict:
                    return HttpStatusCode.Conflict;
                case ServiceResponseStatuses.Unauthorized:
                    return HttpStatusCode.Unauthorized;
                case ServiceResponseStatuses.Forbidden:
                    return HttpStatusCode.Forbidden;
                case ServiceResponseStatuses.ValidationFailed:
                    return HttpStatusCode.BadRequest;
                case ServiceResponseStatuses.NotFound:
                    return HttpStatusCode.NotFound;
                case ServiceResponseStatuses.UnavailableOrBusy:
                    return HttpStatusCode.UnprocessableEntity;
                case ServiceResponseStatuses.NotAssociated:
                    return HttpStatusCode.UnprocessableEntity;
                default:
                    return HttpStatusCode.NotImplemented;
            }
        }
    }

    public enum ServiceResponseStatuses
    {
        Success,
        Conflict,
        Unauthorized,
        Forbidden,
        [Description("Validation_Failed")]
        ValidationFailed,
        [Description("Not_Found")]
        NotFound,
        [Description("Unavailable/Busy")]
        UnavailableOrBusy,
        [Description("Not_Associated")]
        NotAssociated
    }
}