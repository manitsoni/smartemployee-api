using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class VendorSQL
    {
        readonly SqlConnection con;

        public VendorSQL(DatabaseConnection databaseConnection)
        {
            con = new SqlConnection(databaseConnection.ConnectionString);
        }

        public int AddEditVendor(VendorEntity objVendor)
        {
            int ID = 0;
            SqlCommand cmd = new SqlCommand("Vendor_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VendorID", objVendor.VendorID);
            cmd.Parameters.AddWithValue("@VendorName", objVendor.VendorName);
            cmd.Parameters.AddWithValue("@Place", objVendor.Place);
            cmd.Parameters.AddWithValue("@MobileNo", objVendor.MobileNo);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return ID;

        }


        public Int64 DeleteVendor(long VendorID)
        {
            SqlCommand cmd = new SqlCommand("Delete_Vendor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VendorID", VendorID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return VendorID;
        }


        public List<VendorEntity> GetCompany()
        {
            VendorEntity obj = new VendorEntity();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Vendor_Search", con);
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
                return ConvertDatasetToVendor(ds.Tables[0]);
            else
                return new List<VendorEntity>();
        }



        public List<VendorEntity> ConvertDatasetToVendor(DataTable dt)
        {
            List<VendorEntity> lstVendor = new List<VendorEntity>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    VendorEntity objVendor = new VendorEntity();
                    objVendor.VendorID = Convert.ToInt64(dr["VendorID"]);
                    objVendor.VendorName = Convert.ToString(dr["VendorName"]);
                    objVendor.Place = Convert.ToString(dr["Place"]);
                    objVendor.MobileNo = Convert.ToString(dr["MobileNo"]);
                    lstVendor.Add(objVendor);
                }
            }
            return lstVendor;
        }
    }
}
