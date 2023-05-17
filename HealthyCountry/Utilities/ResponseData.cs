using System.Collections.Generic;

namespace HealthyCountry.Utilities
{
    public class ResponseData : ResponseData<object>
    {
        public ResponseData()
        {}
        public ResponseData(object data) : base(data){}
    }
    public class ResponseData<T>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public T Data { get; set; }
        public Dictionary<string, object> Meta { get; set; }
        public Dictionary<string, object> Populated { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public PaginationResponse Paging { get; set; }
        
        // ReSharper disable once MemberCanBeProtected.Global
        public ResponseData(){}

        // ReSharper disable once MemberCanBeProtected.Global
        public ResponseData(T data)
        {
            Data = data;
        }

        public void AddPaginationData(long length, int page, int? limit = null)
        {
            Paging = new PaginationResponse(length, page, limit);
        }
        public void AddPaginationData(PaginationResponse paging)
        {
            Paging = paging;
        }
        
        
        public class PaginationResponse
        {
            // ReSharper disable once MemberCanBePrivate.Global
            public long Length { get; internal set; }
            // ReSharper disable once MemberCanBePrivate.Global
            public int Page { get; }
            // ReSharper disable once MemberCanBePrivate.Global
            public int? Limit { get; }

            public PaginationResponse(long length, int page = 1, int? limit = null)
            {
                Length = length;
                Page = page;
                Limit = limit;
            }
        }
    }
}