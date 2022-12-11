using AutoMapper;
using Exam.Core.DTOs;
using Exam.Core.Interfaces;
using Exam.Core.Utility;
using Exam.Domain.Entities;
using Exam.Domain.Enums;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI.Common;
using System.Data;
using System.Net;
using System.Text;

namespace Exam.Core.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQrCodeService _qrCodeService;
        public string baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        public CandidateService(IMapper mapper, IUnitOfWork unitOfWork, IQrCodeService qrCodeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _qrCodeService = qrCodeService;
        }
        /// <summary>
        /// The method handle candidates excel sheep upload, it extracts the data and store it in the database
        /// </summary>
        /// <param name="candidatesSheet"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<CandidateDto>>> UploadExcelSheet(IFormFile candidatesSheet)
        {
            try
            {
                var path = Utitity.GetFilePath(baseDirectory, $"ExamCandidatesTracker.Infrastructure\\ExcelSheet\\{candidatesSheet.FileName}");
                FileInfo fileInfo = new(path);
                if (!fileInfo.Exists)
                {
                    fileInfo.Directory.Create();
                }
                File.Delete(path);
                using (FileStream stream = new(path, FileMode.CreateNew))
                {
                    await candidatesSheet.CopyToAsync(stream);
                }
                DataSet dataSet;
                try
                {
                    List<CandidateDto> newlyAddedCandidates = new List<CandidateDto>();
                    FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    dataSet = reader.AsDataSet(
                        new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = false,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        //Maps Excel rows to Candidate entity 
                        Candidate candidate = new()
                        {
                            FullName = dataSet.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) : "",
                            Gender = dataSet.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]) : "",
                            Address = dataSet.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[2]) : "",
                            Email = dataSet.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[3]) : "",
                            PhoneNumber = dataSet.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[4]) : "",
                            ExamNumber = dataSet.Tables[0].Rows[i].ItemArray[5] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[5]) : "",
                            ExamDateTime = dataSet.Tables[0].Rows[i].ItemArray[6] != null ? Convert.ToDateTime(dataSet.Tables[0].Rows[i].ItemArray[6]) : DateTime.Now,
                            CentreCode = dataSet.Tables[0].Rows[i].ItemArray[7] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[7]) : ""
                        };
                        //Checks if the candidate exam number already exist in the database.
                        var alreadyExist = await DoesCandidateExist(candidate.ExamNumber);

                        if (!alreadyExist && candidate.CentreCode == CenterCode.EC2359.ToString())
                        {
                            // Add candidate record into database 
                            await _unitOfWork.CandidateRepository.Insert(candidate);
                            await _unitOfWork.Save();

                            var mapped = _mapper.Map<CandidateDto>(candidate);
                            newlyAddedCandidates.Add(mapped);

                            //Calls Generate QRCode service passing in the candidate data to Generate the QRCode.
                            await _qrCodeService.GenerateQrCodeForCandidates(candidate);
                        }
                    }
                    stream.Close();
                        
                    File.Delete(path);

                    if (newlyAddedCandidates.Count == 0) return ResponseDto<List<CandidateDto>>.Success("Candidates already uploaded", newlyAddedCandidates, (int)HttpStatusCode.OK);

                    return ResponseDto<List<CandidateDto>>.Success($"{newlyAddedCandidates.Count} candidates uploaded successfully and QRCode sent to the candidates email!", newlyAddedCandidates, (int)HttpStatusCode.OK);
                    
                }
                catch (Exception)
                {
                    return ResponseDto<List<CandidateDto>>.Fail("Failed to Upload", (int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception)
            {
                return ResponseDto<List<CandidateDto>>.Fail("Failed to Upload", (int)HttpStatusCode.BadRequest);
            }

        }
        /// <summary>
        /// Gets all the candidates in the system.
        /// </summary>
        /// <returns></returns>
        public  async Task<ResponseDto<List<CandidateDto>>> GetAllCandidates()
        {
            try
            {
                var result = new List<CandidateDto>();
                var allCandidates = await _unitOfWork.CandidateRepository.GetAll(Includes: new List<string>() { "Attendance"});
                if(allCandidates.Count != 0)
                {
                    foreach (var record in allCandidates)
                    {
                        var mapped = _mapper.Map<CandidateDto>(record);

                        result.Add(mapped);
                    }
                    return ResponseDto<List<CandidateDto>>.Success($"There are {result.Count} registered candidates",result, (int)HttpStatusCode.OK);
                }
                return ResponseDto<List<CandidateDto>>.Fail("No Candidate available", (int)HttpStatusCode.NotFound);
            }catch(Exception)
            {
                return ResponseDto<List<CandidateDto>>.Fail("Unable to Get all candidates", (int)HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// method to check if the Candidate Already Exist
        /// </summary>
        /// <param name="examNumber"></param>
        /// <returns></returns>
        private async Task<bool> DoesCandidateExist(string examNumber)
        {
            try
            {
                //Check If the Candidate Already Exist
                var candidate = await _unitOfWork.CandidateRepository.Get(x => x.ExamNumber == examNumber);
                if (candidate != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// method to delete all candidates
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> DeleteAllCandidates()
        {
            try
            {
                var allCandidate = await _unitOfWork.CandidateRepository.GetAll();
                if (allCandidate.Count != 0)
                {
                    _unitOfWork.CandidateRepository.DeleteRange(allCandidate);
                    await _unitOfWork.Save();
                    return ResponseDto<bool>.Success($"{allCandidate.Count} Candidates Successfully Deleted!", true, (int)HttpStatusCode.OK);
                }
                else
                {
                    return ResponseDto<bool>.Fail("No Availale Candidate",(int)HttpStatusCode.NotFound);
                }
            }
            catch(Exception)
            {
                return ResponseDto<bool>.Fail("There An Issue deleting all Candidates", (int)HttpStatusCode.BadRequest);
            }
        }
    }
}