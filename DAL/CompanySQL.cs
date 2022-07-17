using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CompanySQL
    {
        readonly SqlConnection con;

        public CompanySQL(DatabaseConnection databaseConnection)
        {
            con = new SqlConnection(databaseConnection.ConnectionString);
        }

        public int AddEditCompany(int CompanyID, string CompnayName)
        {
            SqlCommand cmd = new SqlCommand("Company_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("@CompanyName", CompnayName);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return CompanyID;
        }


        public int DeleteCompany(int CompanyID)
        {
            SqlCommand cmd = new SqlCommand("Delete_Company", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return CompanyID;
        }


        public List<CompanyEntity> GetCompany()
        {
            CompanyEntity obj = new CompanyEntity();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Company_Search", con);
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
                return ConvertDatasetToComp(ds.Tables[0]);
            else
                return new List<CompanyEntity>();
        }


        public List<CompanyEntity> ConvertDatasetToComp(DataTable dt)
        {
            List<CompanyEntity> lstCompany = new List<CompanyEntity>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CompanyEntity objCompany = new CompanyEntity();
                    objCompany.CompanyID = Convert.ToInt32(dr["CompanyID"]);
                    objCompany.CompanyName = Convert.ToString(dr["CompanyName"]);
                    lstCompany.Add(objCompany);
                }
            }
            return lstCompany;
        }
    }
}
