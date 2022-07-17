using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PurchaseDetailSQL
    {
        readonly SqlConnection con;

        public PurchaseDetailSQL(DatabaseConnection databaseConnection)
        {
            con = new SqlConnection(databaseConnection.ConnectionString);
        }

        public int AddEditPurchaseDetail(PurchaseDetailEntity obj)
        {
            int id = 0;
            SqlCommand cmd = new SqlCommand("PurchaseDetail_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PurchaseDetailID", obj.PurchaseDetailID);
            cmd.Parameters.AddWithValue("@PurchaseMainID", obj.PurchaseMainID);
            cmd.Parameters.AddWithValue("@Photo", obj.Photo);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return id;

        }

        public int DeletePurchaseDetail(long PurchaseDetailID)
        {
            int id = 0;
            SqlCommand cmd = new SqlCommand("Delete_PurchaseDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PurchaseDetailID", PurchaseDetailID);
            con.Open();
            id = cmd.ExecuteNonQuery();
            con.Close();
            return id;
        }


        public List<PurchaseDetailEntity> GetPurchaseDetail()
        {
            PurchaseDetailEntity obj = new PurchaseDetailEntity();
            DataSet ds = new DataSet();
            try
            {

                string abc = "image1, image2, image3";
                obj.lstPhotos = new List<string>(abc.Split(","));
                SqlCommand cmd = new SqlCommand("PurchaseDetail_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                con.Open();
                sd.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (ds != null && ds.Tables.Count > 0)
                return ConvertDatasetToPurchaseDetail(ds.Tables[0]);
            else
                return new List<PurchaseDetailEntity>();
        }



        public List<PurchaseDetailEntity> ConvertDatasetToPurchaseDetail(DataTable dt)
        {
            List<PurchaseDetailEntity> lstPurchaseDetail = new List<PurchaseDetailEntity>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PurchaseDetailEntity objPurchaseDetail = new PurchaseDetailEntity();
                    objPurchaseDetail.PurchaseDetailID = Convert.ToInt64(dr["PurchaseDetailID"]);
                    objPurchaseDetail.Photo = Convert.ToString(dr["Photo"]);
                    if (!string.IsNullOrEmpty(objPurchaseDetail.Photo))
                    {
                        objPurchaseDetail.lstPhotos = new List<string>(objPurchaseDetail.Photo.Split(","));
                    }
                    else
                        objPurchaseDetail.lstPhotos = new List<string>();

                    lstPurchaseDetail.Add(objPurchaseDetail);
                }
            }
            return lstPurchaseDetail;
        }
    }
}
