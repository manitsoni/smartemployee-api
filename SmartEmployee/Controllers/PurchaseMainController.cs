using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace SmartEmployee.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class PurchaseMainController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly PurchaseMainSQL purchaseMainSQL;
        public const string _purchaseMainFileUploadFolder = "PurchaseMain";
        public const string _tempFile = "TempFile";

        public PurchaseMainController(IWebHostEnvironment environment, PurchaseMainSQL purchaseMainSQL)
        {
            this._environment = environment;
            this.purchaseMainSQL = purchaseMainSQL;
        }

        [HttpPost("AddPurchaseMain")]
        public PurchaseMainResult AddPurchaseMain([FromBody]PurchaseMainEntity objPurchaseMain)
        {
            PurchaseMainResult result = new PurchaseMainResult();

            result.IsSuccess = true;

            try
            {
                var id = purchaseMainSQL.AddEditPurchaseMain(objPurchaseMain);

                if (objPurchaseMain.PurchaseMainID <= 0)
                    result.Message = "PurchaseMain Added Successfully.";
                else
                    result.Message = "PurchaseMain Update Successfully.";

                objPurchaseMain.PurchaseMainID = id;
                result.lstPurchaseMain = new List<PurchaseMainEntity> { objPurchaseMain };
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }

            return (result);
        }

        [HttpGet("GetPurchaseMain")]
        public string GetPurchaseMain()
        {
            PurchaseMainResult result = new PurchaseMainResult();
            var Result = new { result };
            result.IsSuccess = true;

            try
            {
                result.lstPurchaseMain = purchaseMainSQL.GetPurchaseMain();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
            }

            return JsonConvert.SerializeObject(Result);
        }

        [HttpPost("UploadFiles/{id}")]
        public async Task<IActionResult> UploadFiles(int id, List<IFormFile> postedFiles)
        {
            PurchaseMainResult result = new();

            string path = Path.Combine(_environment.WebRootPath, _purchaseMainFileUploadFolder, id.ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new();

            try
            {
                foreach (IFormFile postedFile in postedFiles)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);

                    using FileStream stream = new(Path.Combine(path, fileName), FileMode.Create);

                    postedFile.CopyTo(stream);

                    uploadedFiles.Add(fileName);
                }
            }
            catch(Exception ex)
            {
                result.Message = ex.Message + Environment.NewLine + ex.StackTrace;

                return Ok(result);
            }

            result.IsSuccess = true;

            return Ok(result);
        }

        [HttpPost("GetFiles/{id}")]
        public async Task<IActionResult> GetFiles(int id)
        {
            PurchaseMainResult result = new();

            string path = Path.Combine(_environment.WebRootPath, _purchaseMainFileUploadFolder, id.ToString());

            if (!Directory.Exists(path))
            {
                result.Message = "Files doesn't exist";
                return Ok(result);
            }

            try
            {
                string[] fileArray = Directory.GetFiles(path).Select(x => Path.GetFileName(x)).ToArray();

                return Ok(fileArray);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message + Environment.NewLine + ex.StackTrace;

                return Ok(result);

            }

            result.IsSuccess = true;

            return Ok(result);
        }

        [HttpPost("DownloadFiles/{id}")]
        public async Task<IActionResult> DownloadFiles(int id)
        {
            PurchaseMainResult result = new();

            string path = Path.Combine(_environment.WebRootPath, _purchaseMainFileUploadFolder, id.ToString());
            var fileName = $"{_tempFile}\\{_purchaseMainFileUploadFolder}_{id}_{Guid.NewGuid()}.zip";
            string zipPath = Path.Combine(_environment.WebRootPath, fileName);

            if (!Directory.Exists(path))
            {
                result.Message = "Files doesn't exist";
                return Ok(result);
            }

            try
            {
                ZipFile.CreateFromDirectory(path,
                                            zipPath,
                                            compressionLevel: CompressionLevel.SmallestSize,
                                            includeBaseDirectory: false,
                                            entryNameEncoding: Encoding.UTF8);

                return File(fileName, GetContentType(zipPath), zipPath);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message + Environment.NewLine + ex.StackTrace;

                return Ok(result);
            }
            finally
            {
                //Delete file after 1 minute from temp folder
                Task.Run(() => 
                {
                    Thread.Sleep(60000);
                    System.IO.File.Delete(zipPath);
                });
            }

            result.IsSuccess = true;

            return Ok(result);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();

            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        [HttpDelete("DeletePurchaseMain/{deleteFile?}")]
        public PurchaseMainResult DeletePurchaseMain([FromBody]PurchaseMainEntity obj, bool deleteFile = true)
        {
            PurchaseMainResult result = new PurchaseMainResult();
            result.IsSuccess = true;
            try
            {
                if (obj.PurchaseMainID > 0)
                {
                    purchaseMainSQL.DeletePurchaseMain(obj.PurchaseMainID);

                    if (deleteFile)
                    {
                        string path = Path.Combine(_environment.WebRootPath, _purchaseMainFileUploadFolder, obj.PurchaseMainID.ToString());

                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                        }
                    }
                }
                else
                {
                    result.Message = "Select PurchaseMain";
                    result.IsSuccess = false;
                }
                    
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
            }
            return (result);
        }

    }
}