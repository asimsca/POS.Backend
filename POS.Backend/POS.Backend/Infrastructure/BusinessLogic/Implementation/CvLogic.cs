using System.Net;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.POS;
using POS.Backend.DTO.Response.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace POS.Backend.Infrastructure.BusinessLogic.Implementation
{
    public class POSLogic : IPOSLogic
    {
        private readonly IPOSDb POSDb;
        private readonly LogHelper logHelper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public POSLogic(IPOSDb POSDb, LogHelper logHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.POSDb = POSDb;
            this.logHelper = logHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> AddPOS(AddPOSRequest request)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            try
            {
                baseResponse = await this.POSDb.AddPOS(request);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                this.logHelper.LogException(request, ex);
            }
            return baseResponse;
        }

        public async Task<BaseResponse<string>> UploadProfilePicture(IFormFile file)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            try
            {
                if (file == null || file.Length == 0)
                {
                    baseResponse.IsSuccess = false;
                    baseResponse.Message = "No file uploaded.";
                    return baseResponse;
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/profile-pics");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/profile-pics/{uniqueFileName}"; // relative path for DB

                baseResponse.IsSuccess = true;
                baseResponse.Data = fileUrl;
                baseResponse.Message = "Upload successful";
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                this.logHelper.LogException(file, ex);
            }

            return baseResponse;
        }


        public async Task<BaseResponse<string>> UploadPdf(IFormFile file)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            try
            {
                if (file == null || file.Length == 0)
                {
                    baseResponse.IsSuccess = false;
                    baseResponse.Message = "No file uploaded.";
                    return baseResponse;
                }
                
                 var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "POSs");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Build URL to access file
                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var fileUrl = $"{baseUrl}/uploads/POSs/{fileName}";

                baseResponse.IsSuccess = true;
                baseResponse.Data = fileUrl;
                baseResponse.Message = "File Upload successful";

                await this.POSDb.AddPOSFIle(baseResponse.Data);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                this.logHelper.LogException(file, ex);
            }

            return baseResponse;
        }


    }
}
