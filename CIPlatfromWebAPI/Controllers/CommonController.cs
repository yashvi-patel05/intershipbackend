using Business_logic_Layer;
using Data_Access_Layer.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly BALCommon _balCommon;
        private readonly ResponseResult result = new ResponseResult();
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public CommonController(BALCommon balCommon, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _balCommon = balCommon;
            _environment = environment;
        }
        [HttpGet]
        [Route("CountryList")]
        public async Task<IActionResult> CountryList()
        {
            try
            {
                var result = await _balCommon.GetCountryListAsync();
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("CityList/{countryId}")]
        public async Task<IActionResult> CityList(int countryId)
        {
            try
            {
                var result = await _balCommon.GetCityListAsync(countryId);
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("MissionCountryList")]
        public async Task<IActionResult> MissionCountryList()
        {
            try
            {
                var result = await _balCommon.GetMissionCountryListAsync();
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("MissionCityList")]
        public async Task<IActionResult> MissionCityList()
        {
            try
            {
                var result = await _balCommon.GetMissionCityListAsync();
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("MissionThemeList")]
        public async Task<IActionResult> MissionThemeList()
        {
            try
            {
                var result = await _balCommon.GetMissionThemeListAsync();
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("MissionSkillList")]
        public async Task<IActionResult> MissionSkillList()
        {
            try
            {
                var result = await _balCommon.GetMissionSkillListAsync();
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("MissionTitleList")]
        public async Task<IActionResult> MissionTitleList()
        {
            try
            {
                var result = await _balCommon.GetMissionTitleListAsync();
                return Ok(new { status = "Success", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }


        // this should be in common controller
        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] Upload upload)//upload class which gives file path :)
        {
            try
            {
                string filePath = "";
                string fullPath = "";
                var files = Request.Form.Files;
                List<string> fileList = new List<string>();
                if (files == null && files.Count <= 0)
                {
                    return BadRequest(new ResponseResult() { Message = "Please upload necessary file", Result = ResponseStatus.Success });
                }
                foreach (var file in files)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filePath = Path.Combine("Uploads", upload.ModuleName); // Mission, MissionDoc, userskill smn like that idk
                    string fileRootPath = Path.Combine(_environment.WebRootPath, filePath);

                    if (!Directory.Exists(fileRootPath))
                    {
                        Directory.CreateDirectory(fileRootPath);
                    }

                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string extenxion = Path.GetExtension(fileName);
                    string fullFileName = name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extenxion;
                    fullPath = Path.Combine(fileRootPath, fullFileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    fileList.Add(fullPath);
                }
                return Ok(new ResponseResult() { Data = fileList, Result = ResponseStatus.Success });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult() { Message = ex.Message, Result = ResponseStatus.Success });
            }
        }
    }
}