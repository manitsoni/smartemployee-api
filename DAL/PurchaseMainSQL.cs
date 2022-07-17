using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PurchaseMainSQL
    {
        readonly SqlConnection con;

        public PurchaseMainSQL(DatabaseConnection databaseConnection)
        {
            con = new SqlConnection(databaseConnection.ConnectionString);
        }

        public long AddEditPurchaseMain(PurchaseMainEntity obj)
        {
            long id;
            SqlCommand cmd = new SqlCommand("PurchaseMain_AddEdit", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PurchasemainID", obj.PurchaseMainID);
            cmd.Parameters.AddWithValue("@PurchaseDate", obj.PurchaseDate.Date);
            cmd.Parameters.AddWithValue("@VendorID", obj.VendorID);
            cmd.Parameters.AddWithValue("@VendorName", obj.VendorName);
            cmd.Parameters.AddWithValue("@ToBillAmt", obj.ToBillAmt);
            cmd.Parameters.AddWithValue("@CompanyID", obj.CompanyID);
            cmd.Parameters.AddWithValue("@UserID", obj.UserID);
            cmd.Parameters.AddWithValue("@Type", obj.Type);

            // Get id back from stored procedure
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();

            // parse id
            id = long.Parse(cmd.Parameters["@id"].Value.ToString());

            con.Close();
            return id;

        }

        public Int64 DeletePurchaseMain(long PurchaseMainID)
        {
            SqlCommand cmd = new SqlCommand("Delete_PurchaseMain", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PurchaseMainID", PurchaseMainID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return PurchaseMainID;
        }

        public List<PurchaseMainEntity> GetPurchaseMain()
        {
            PurchaseMainEntity obj = new PurchaseMainEntity();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("PurchaseMain_Search", con);
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
                return ConvertDatasetToPurchaseMain(ds.Tables[0]);
            else
                return new List<PurchaseMainEntity>();
        }



        public List<PurchaseMainEntity> ConvertDatasetToPurchaseMain(DataTable dt)
        {
            List<PurchaseMainEntity> lstobjPurchaseMain = new List<PurchaseMainEntity>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PurchaseMainEntity objPurchaseMain = new PurchaseMainEntity();
                    objPurchaseMain.PurchaseMainID = Convert.ToInt32(dr["PurchaseMainID"]);
                    objPurchaseMain.CompanyID = Convert.ToInt32(dr["CompanyID"]);
                    objPurchaseMain.VendorID = Convert.ToInt64(dr["VendorID"]);
                    objPurchaseMain.VendorName = Convert.ToString(dr["VendorName"]);
                    objPurchaseMain.PurchaseDate = Convert.ToDateTime(dr["PurchaseDate"]);
                    objPurchaseMain.ToBillAmt = Convert.ToDecimal(dr["ToBillAmt"]);
                    objPurchaseMain.UserID = Convert.ToInt32(dr["UserID"]);
                    objPurchaseMain.Type = Convert.ToString(dr["Type"]);
                    //objPurchaseMain.UserName = Convert.ToString(dr["UserName"]);
                    //objPurchaseMain.CompanyName = Convert.ToString(dr["CompanyName"]);
                    lstobjPurchaseMain.Add(objPurchaseMain);
                }
            }
            return lstobjPurchaseMain;
        }

    }
}
