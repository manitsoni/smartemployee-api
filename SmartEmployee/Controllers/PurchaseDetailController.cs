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
    public class PurchaseDetailController : Controller
    {
        private readonly PurchaseDetailSQL purchaseDetailSQL;

        public PurchaseDetailController(PurchaseDetailSQL purchaseDetailSQL)
        {
            this.purchaseDetailSQL = purchaseDetailSQL;
        }

        [HttpPost("AddPurchaseDetail")]
        public PurchaseDetailResult AddPurchaseDetail([FromBody]PurchaseDetailEntity objPurchaseDetail)
        {
            PurchaseDetailResult result = new PurchaseDetailResult();
            result.IsSuccess = true;
            try
            {
                purchaseDetailSQL.AddEditPurchaseDetail(objPurchaseDetail);
                if (objPurchaseDetail.PurchaseDetailID <= 0)
                    result.Message = "Purchase Detail Added Successfully.";
                else
                    result.Message = "Purchase Detail Update Successfully.";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
            }
            return (result);
        }

        [HttpGet("GetPurchaseDetail")]
        public string GetPurchaseDetail()
        {
            PurchaseDetailResult result = new PurchaseDetailResult();

            var Result = new { result };

            result.IsSuccess = true;

            try
            {
                result.lstPurchaseDetail = purchaseDetailSQL.GetPurchaseDetail();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.ToString();
            }

            return JsonConvert.SerializeObject(Result);
        }

        [HttpDelete("DeletePurchase")]
        public PurchaseDetailResult DeletePurchase([FromBody]PurchaseDetailEntity obj)
        {
            PurchaseDetailResult result = new PurchaseDetailResult();
            result.IsSuccess = true;
            try
            {
                if (obj.PurchaseDetailID > 0)
                    purchaseDetailSQL.DeletePurchaseDetail(obj.PurchaseDetailID);
                else
                {
                    result.Message = "Select Purchase";
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