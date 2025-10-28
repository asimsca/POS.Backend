using System.Net;

namespace POS.Backend.DTO
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            this.IsSuccess = false;
            this.ResponseCode = HttpStatusCode.OK;
            this.Message = string.Empty;
            this.Data = default;
        }

        public bool IsSuccess { get; set; }
        public HttpStatusCode ResponseCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
}
