using Exam.Core.DTOs;
using Exam.Core.Interfaces;
using ExamCandidatesTracker.Core.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Exam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly IQrCodeService _qrCodeService;

        public CandidateController(ICandidateService candidateService, IQrCodeService barcodeService)
        {
            _candidateService = candidateService;
            _qrCodeService = barcodeService;
        }
        /// <summary>
        /// Uploads Excel file of the candidates
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-candidates-excel-sheet")]
        public async Task<IActionResult> UploadCandidatesFile(IFormFile file)
        {
            var validate = new ExcelFileValidator().Validate(file);
            if(validate.IsValid)
            {
                var result = await _candidateService.UploadExcelSheet(file);
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode((int)HttpStatusCode.BadRequest, validate);
        }
        /// <summary>
        /// Checks if the Candidate is Registered
        /// </summary>
        /// <param name="candidatesQrCodeDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("verify-candidate-qrcode")]
        public async Task<IActionResult> VerifyCandidateQrCode(CandidatesQrcodeDetailsDto candidatesQrCodeDetails)
        {
            var result = await _qrCodeService.VerifyCandidateQrCodeDetails(candidatesQrCodeDetails);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Gets all registered candidates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-candidates")]
        public async Task<IActionResult> GetAllCandidates()
        {
            var result = await _candidateService.GetAllCandidates();
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Deletes all the candidates
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("delete-all-candidates")]
        public async Task<IActionResult> DeleteAllCandidate()
        {
            var result = await _candidateService.DeleteAllCandidates();
            return StatusCode(result.StatusCode, result);
        }
    }
}
