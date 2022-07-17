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
    public class VendorController : Controller
    {
        private readonly VendorSQL vendorSQL;

        public VendorController(VendorSQL vendorSQL)
        {
            this.vendorSQL = vendorSQL;
        }

        [HttpPost("AddVendor")]
        public VendorResult AddVendor([FromBody]VendorEntity objVendor)
        {
            VendorResult result = new VendorResult();
            result.IsSuccess = true;
            try
            {
                vendorSQL.AddEditVendor(objVendor);
                if (objVendor.VendorID <= 0)
                    result.Message = "Vendor Added Successfully.";
                else
                    result.Message = "VendorUPdate Successfully.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
            }
            return (result);
        }

        [HttpGet("GetVendors")]
        public string GetVendors()
        {
            VendorResult result = new VendorResult();
            var Result = new { result };
            result.IsSuccess = true;
            try
            {
                result.lstVendor = vendorSQL.GetCompany();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
            }
            return JsonConvert.SerializeObject(Result);
        }

        [HttpDelete("DeleteVendor")]
        public VendorResult DeleteVendor([FromBody]VendorEntity obj)
        {
            VendorResult result = new VendorResult();
            result.IsSuccess = true;
            try
            {
                if (obj.VendorID > 0)
                    vendorSQL.DeleteVendor(obj.VendorID);
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Select Vendor";
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