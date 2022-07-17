using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UsersSQL
    {
        readonly SqlConnection con;

        public UsersSQL(DatabaseConnection databaseConnection)
        {
            con = new SqlConnection(databaseConnection.ConnectionString);
        }

        public int AddEditUser(UsersEntity objUser)
        {
            int id = 0;

            SqlCommand cmd = new SqlCommand("User_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", objUser.UserID);
            cmd.Parameters.AddWithValue("@UserName", objUser.UserName);
            cmd.Parameters.AddWithValue("@CompanyID", objUser.CompanyID);
            cmd.Parameters.AddWithValue("@Password", objUser.Password);
            cmd.Parameters.AddWithValue("@IsMultiUser", objUser.IsMultiUser);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return id;
        }



        public int DeleteUser(int UserID)
        {
            SqlCommand cmd = new SqlCommand("Delete_Users", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return UserID;
        }




        public int UserLogin(string UserName, string Password, int Value)
        {
            SqlCommand cmd = new SqlCommand("User_Login", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);
            con.Open();
            Value = Convert.ToInt16(cmd.ExecuteScalar());
            con.Close();
            return Value;
        }

        public List<UsersEntity> GetUsers()
        {
            UsersEntity obj = new UsersEntity();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Users_Search", con);
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
                return ConvertDatasetToUser(ds.Tables[0]);
            else
                return new List<UsersEntity>();
        }


        public List<UsersEntity> ConvertDatasetToUser(DataTable dt)
            {
            List<UsersEntity> lstUser = new List<UsersEntity>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UsersEntity objUser = new UsersEntity();
                    objUser.UserID = Convert.ToInt32(dr["UserID"]);
                    objUser.UserName = Convert.ToString(dr["UserName"]);
                    objUser.Password = Convert.ToString(dr["Password"]);
                    objUser.CompanyID = Convert.ToInt32(dr["CompanyID"]);
                    objUser.IsMultiUser = Convert.ToBoolean(dr["IsMultiUser"]);
                    lstUser.Add(objUser);
                }
            }
            return lstUser;
        }


        public int LoginCheck(UsersEntity ul)
        {
            SqlCommand com = new SqlCommand("Sp_UserLogin", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@LoginID", ul.UserName);
            com.Parameters.AddWithValue("@Password", ul.Password);
            SqlParameter objULogin = new SqlParameter();
            objULogin.ParameterName = "@Isvalid";
            objULogin.SqlDbType = SqlDbType.Bit;
            objULogin.Direction = ParameterDirection.Output;
            com.Parameters.Add(objULogin);
            con.Open();
            com.ExecuteNonQuery();
            int res = Convert.ToInt32(objULogin.Value);
            con.Close();
            return res;


        }

    }
}
