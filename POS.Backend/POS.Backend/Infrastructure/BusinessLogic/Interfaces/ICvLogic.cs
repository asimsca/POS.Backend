using POS.Backend.DTO.Request.POS;
using POS.Backend.DTO;
using Microsoft.AspNetCore.Mvc;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface IPOSLogic
    {
        Task<BaseResponse<string>> AddPOS(AddPOSRequest request);
        Task<BaseResponse<string>> UploadProfilePicture(IFormFile file);
        Task<BaseResponse<string>> UploadPdf(IFormFile file);
    }
}
