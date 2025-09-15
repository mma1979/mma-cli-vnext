namespace MmaSolution.Core.Models
{
    public class ResultViewModel<T>
    {
        public long Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public T Data { get; set; }
        public List<string> Messages { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public ResultViewModel()
        {
            Messages = new List<string>();
        }

        public static ResultViewModel<T> Error(List<string> messages=default)
        {
            return new ResultViewModel<T>
            {
                Messages = messages,
                IsSuccess = false,
                StatusCode = 500
            };
        }

        public static ResultViewModel<T> Success(T data,long total=default,int pageNumber=1, int pageSize=10 ,List<string> messages = default)
        {
            return new ResultViewModel<T>
            {
                Data  = data,
                Total = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Messages = messages,
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }

    public class AcknowledgeViewModel
    {
        public List<string> Messages { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }


        public AcknowledgeViewModel()
        {
            Messages = new List<string>();
        }

        public static AcknowledgeViewModel Error(List<string> messages = default)
        {
            return new AcknowledgeViewModel()
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = messages
            };
        }

        public static AcknowledgeViewModel Success(List<string> messages = default)
        {
            return new AcknowledgeViewModel()
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = messages
            };
        }
    }
}
