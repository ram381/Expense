using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Business_Layer
{
    public class ExpenseDAL
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["expensecon"].ConnectionString;
            }
            set { }
        }

        public List<ExpenseModel> GetExpenses()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            List<ExpenseModel> exp = new List<ExpenseModel>();
            DataSet ds = new DataSet(); 
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllValues",con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    exp.Add(new ExpenseModel()
                    {
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["expense_Id"]),
                        Amount = Convert.ToDouble(ds.Tables[0].Rows[i]["amount"]),
                        Name = Convert.ToString(ds.Tables[0].Rows[i]["Name"]),
                        Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]),
                        Description = Convert.ToString(ds.Tables[0].Rows[i]["Description"]),
                        Cat = new CategoryModel()
                        {
                            Id = Convert.ToInt16(ds.Tables[0].Rows[i]["Category_Id"]),
                            Name =Convert.ToString(ds.Tables[0].Rows[i]["cat_name"])
                        }                      
                    });
                }
            };
            return exp;
        }
        public List<CategoryModel> GetCat()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            List<CategoryModel> cat = new List<CategoryModel>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select id as Id , name as name from tblcategory ", con);
                con.Open();                 
                da.SelectCommand = cmd;
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cat.Add(new CategoryModel()
                    {                         
                            Id = Convert.ToInt16(ds.Tables[0].Rows[i]["Id"]),
                            Name = Convert.ToString(ds.Tables[0].Rows[i]["name"])                        
                    });
                }
            };
            return cat;
        }
        public int GetCatByName(string name)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            CategoryModel cat = new CategoryModel();
            DataSet ds = new DataSet();
            object catmodel;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select id as Id from tblcategory where name like '" + name + "'", con);
                con.Open();
                catmodel = cmd.ExecuteScalar();                                
            };
            return Convert.ToInt32(catmodel);
        }

        public List<CategoryModel> SaveCat(string name)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            List<CategoryModel> cat = new List<CategoryModel>();
            cat = GetCat();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Insert into tblcategory(id,Name) values(@id,@Name)", con);
                con.Open();
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@Name",
                    Value = name
                });
                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@id",
                    Value = cat.Count +1
                });
                int change=  cmd.ExecuteNonQuery();
                if(change > 0)
                {
                    cat = GetCat();
                }
            };
            return cat;
        }

        public List<ExpenseModel> SaveExpense(ExpenseModel exp)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            List<ExpenseModel> cat = new List<ExpenseModel>();
            DataSet ds = new DataSet();
            exp.Cat.Id= Convert.ToInt32(GetCatByName(exp.Cat.Name));
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_SaveExpense", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name",exp.Name);
                cmd.Parameters.AddWithValue("@Des",exp.Description);
                cmd.Parameters.AddWithValue("@date",exp.Date);
                cmd.Parameters.AddWithValue("@amount",exp.Amount);
                cmd.Parameters.AddWithValue("@charid", exp.Cat.Id);
                 int change =cmd.ExecuteNonQuery();
                if(change >0 )
                {
                    cat= GetExpenses();
                }               
            };
            return cat;
        }
      }
}