using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SmartEmployee.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly CompanySQL companySQL;

        public CompanyController(CompanySQL companySQL)
        {
            this.companySQL = companySQL;
        }

        [HttpPost("AddCompany")]
        public CompanyResult AddCompany([FromBody]CompanyEntity objCompany)
        {
            CompanyResult result = new CompanyResult();
            result.IsSuccess = true;
            try
            {
                companySQL.AddEditCompany(objCompany.CompanyID, objCompany.CompanyName);
                if (objCompany.CompanyID <= 0)
                    result.Message = "Company Added Successfully.";
                else
                    result.Message = "Company Update Successfully.";
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return (result);
        }

        [HttpGet("GetCompany")]
        public string GetCompany()
        {
            CompanyResult result = new CompanyResult();
            var Result = new { result };
            result.IsSuccess = true;
            try
            {
                result.lstCompany = companySQL.GetCompany();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return JsonConvert.SerializeObject(Result);
        }

        [HttpDelete("DeleteCompany")]
        public CompanyResult DeleteCompany([FromBody]CompanyEntity obj)
        {
            CompanyResult result = new CompanyResult();
            result.IsSuccess = true; 
            try
            {
                if (obj.CompanyID > 0)
                    companySQL.DeleteCompany(obj.CompanyID);
                else
                {
                    result.Message = "Select Company";
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return (result);
        }
    }
}