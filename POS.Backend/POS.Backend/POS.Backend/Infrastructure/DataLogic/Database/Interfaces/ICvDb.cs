using POS.Backend.DTO.Request.POS;
using POS.Backend.DTO;
using Microsoft.AspNetCore.Mvc;

namespace POS.Backend.Infrastructure.DataLogic.Database.Interfaces
{
    /// <summary>
    /// IPOSDb.
    /// </summary>
    public interface IPOSDb
    {
        /// <summary>
        /// AddPOS.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse<string>> AddPOS(AddPOSRequest request);

        /// <summary>
        /// AddPOSFIle.
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        Task<BaseResponse<string>> AddPOSFIle(string fileUrl);
    }
}
