using AutoMapper;
using Exam.Core.DTOs;
using Exam.Core.Interfaces;
using Exam.Core.Utility;
using Exam.Domain.Entities;
using Exam.Domain.Enums;
using IronBarCode;
using System.Drawing;
using System.Net;

namespace Exam.Core.Services
{
    public class QrCodeService : IQrCodeService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public readonly IEmailService _emailService;
        public readonly IAttendanceService _attendanceService;
        public string baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        public QrCodeService(IUnitOfWork unitOfWork, IEmailService emailService, IMapper mapper, IAttendanceService attendanceService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _mapper = mapper;
            _attendanceService = attendanceService;
        }
        /// <summary>
        /// Verify Qr Code Details of the Candidate
        /// </summary>
        /// <param name="candidateQrCodeDetail"></param>
        /// <returns></returns>
        public async Task<ResponseDto<CandidateDto>> VerifyCandidateQrCodeDetails(CandidatesQrcodeDetailsDto candidateQrCodeDetail)
        {
            var candidate = await _unitOfWork.CandidateRepository.Get(x => x.ExamNumber == candidateQrCodeDetail.ExamNumber);
            if (candidate != null)
            {
                //Check if the Center Code matches The Exam Center Code
                if (candidateQrCodeDetail.CenterCode == CenterCode.EC2359.ToString())
                {
                    //Check If the Attendance is already checkedIn
                    var alreadyCheckedIn = await _attendanceService.isCandidateAlreadyCheckedIn(candidateQrCodeDetail.ExamNumber);
                    if (alreadyCheckedIn)
                    {
                        //Get candidate record and the attendance
                        candidate = await _unitOfWork.CandidateRepository.Get(x => x.ExamNumber == candidateQrCodeDetail.ExamNumber, includes: new List<string>() { "Attendance" });
                        var candidateDto = _mapper.Map<CandidateDto>(candidate);
                        return ResponseDto<CandidateDto>.Success("Already Verified and Attendance Taken", candidateDto, (int)HttpStatusCode.BadRequest);
                    }

                    //Mark Attendance
                    var attendanceObject = _mapper.Map<AttendanceDto>(candidate);
                    var markAttendance = _attendanceService.MarkAttendance(attendanceObject);
                    if(markAttendance.IsCompleted)
                    {
                        await _unitOfWork.Save();
                        //Get candidate record and the attendance
                        candidate = await _unitOfWork.CandidateRepository.Get(x=>x.ExamNumber==candidateQrCodeDetail.ExamNumber, includes: new List<string>() { "Attendance" });
                        var candidateDto = _mapper.Map<CandidateDto>(candidate);
                        return ResponseDto<CandidateDto>.Success("Successful Verification!", candidateDto, (int)HttpStatusCode.OK);
                    }
                    else
                    {
                        return ResponseDto<CandidateDto>.Fail("Verification Failed!", (int)HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return ResponseDto<CandidateDto>.Fail("Invalid Center Code!", (int)HttpStatusCode.NotFound);
                }
            }
            return ResponseDto<CandidateDto>.Fail("Candidate not Found!", (int)HttpStatusCode.NotFound);
        }
        /// <summary>
        /// The method generates Qr Code using candidate exam number and center code
        /// </summary>
        /// <param name="candidatesQrCodeDetails"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> GenerateQrCodeForCandidates(Candidate candidate)
        {
            try
            {
                var qrCode = QRCodeWriter.CreateQrCode(candidate.ExamNumber+" "+candidate.CentreCode);
                qrCode.SetMargins(20);
                qrCode.ChangeBarCodeColor(Color.Black);
                var path = Utitity.GetFilePath(baseDirectory, $"ExamCandidatesTracker.Infrastructure\\QrCodes\\{candidate.ExamNumber}.png");
                qrCode.SaveAsPng(path);

                var email = new EmailDto()
                {
                    Subject = "Exam QRCode",
                    Body = "<em>Find attached file for your QRCode, print and bring along on the exam day </em>",
                    ToEmail = candidate.Email,
                    FilePath = path
                };

                //Call email service to send generated QR Code to the candidate email
                var isSent = await _emailService.SendEmail(email);
                
            }
            catch (Exception)
            {
                return ResponseDto<string>.Fail("Failed while Generating QrCodes", (int)HttpStatusCode.BadRequest);
            }
            return ResponseDto<string>.Success("QRCode Generated", "Successful", (int)HttpStatusCode.OK);
        }
    }
}
