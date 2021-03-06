﻿using RL.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.InfoPage
{
    public partial class PartsInfoEdit : System.Web.UI.Page
    {
        private int pId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //根据有无传递参数显示表格内容
            if (Request.QueryString.AllKeys.Contains("pid"))
            {
                pId = int.Parse(Request.QueryString["pid"]);
            }
            else 
            {
                lbTime.Visible = false;
                txtInTime.Visible = false;
            }
            if (!IsPostBack)
            {
                BindDrop();
                if (pId != 0)
                {
                    ShowInfo(pId);
                }
            }
        }
        //绑定下来菜单
        public void BindDrop()
        {
            string sql = "SELECT Id ,Name FROM dbo.Supplies";
            DataSet ds = DbHelperSQL.Query(sql);
            ddlFromDep.DataSource = ds;
            ddlFromDep.DataTextField = "Name";
            ddlFromDep.DataValueField = "Id";
            ddlFromDep.DataBind();
        }
        public void ShowInfo(int id) 
        {
          
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("SELECT C.PartId,C.Num,C.PartName,S.Name,C.InTime,C.Quantity,C.UnitPrice");
            sbSql.AppendLine("FROM CarParts AS C INNER JOIN Supplies AS S");
            sbSql.AppendLine("ON C.FromDepartId = S.Id");
            sbSql.AppendLine("WHERE PartId = @PartId;");
            SqlParameter[] pms = {
                                    new SqlParameter("@PartId",SqlDbType.Int)
                                  };
            pms[0].Value = id;
            DataSet ds = DbHelperSQL.Query(sbSql.ToString(),pms);
            txtNum.Text = ds.Tables[0].Rows[0]["Num"].ToString();
            txtPartName.Text = ds.Tables[0].Rows[0]["PartName"].ToString();
            txtInTime.Text = ds.Tables[0].Rows[0]["InTime"].ToString();
            txtQuantity.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
            txtUnitPrice.Text = ds.Tables[0].Rows[0]["UnitPrice"].ToString();
            btnAdd.Text = "修改";

        }
        //添加数据
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
            //获取输入值
            
            //由于带参数进入后，将按钮的文字改变了，所以可以根据文字不同进行不同的操作
            if (btnAdd.Text == "添加")
            {
                string Num = txtNum.Text.ToString();
                string PartName = txtPartName.Text.ToString();
                int FromDepartId = Convert.ToInt32(ddlFromDep.SelectedValue);
                DateTime InTime = System.DateTime.Now;
                int Quantity = Convert.ToInt32(txtQuantity.Text);
                decimal UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO CarParts(Num,PartName,FromDepartId,InTime,Quantity,UnitPrice)");
                sb.AppendLine("VALUES(@Num,@PartName,@FromDepartId,@InTime,@Quantity,@UnitPrice);");
                SqlParameter[] pms = { 
                                    new SqlParameter("@Num",SqlDbType.NVarChar,50),
                                    new SqlParameter("@PartName",SqlDbType.NVarChar,50),
                                    new SqlParameter("@FromDepartId",SqlDbType.Int),
                                    new SqlParameter("@InTime",SqlDbType.DateTime),
                                    new SqlParameter("@Quantity",SqlDbType.Int),
                                    new SqlParameter("@UnitPrice",SqlDbType.Decimal)
                                 };
                pms[0].Value = Num;
                pms[1].Value = PartName;
                pms[2].Value = FromDepartId;
                pms[3].Value = InTime;
                pms[4].Value = Quantity;
                pms[5].Value = UnitPrice;
                DbHelperSQL.ExecuteSql(sb.ToString(), pms);
            }
            else 
            {
                string Num = txtNum.Text.ToString();
                string PartName = txtPartName.Text.ToString();
                int FromDepartId = Convert.ToInt32(ddlFromDep.SelectedValue);
                DateTime InTimeText = Convert.ToDateTime(txtInTime.Text);
                int Quantity = Convert.ToInt32(txtQuantity.Text);
                decimal UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                pId = int.Parse(Request.QueryString["pid"]);
                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.AppendLine("UPDATE CarParts SET");
                sbUpdate.AppendLine("Num = @Num, PartName = @PartName,FromDepartId = @FromDepartId,");
                sbUpdate.AppendLine(" InTime = @InTime,Quantity = @Quantity,UnitPrice = @UnitPrice");
                sbUpdate.AppendLine("WHERE PartId = @PartId;");
                SqlParameter[] pars = { 
                                    new SqlParameter("@Num",SqlDbType.NVarChar,50),
                                    new SqlParameter("@PartName",SqlDbType.NVarChar,50),
                                    new SqlParameter("@FromDepartId",SqlDbType.Int),
                                    new SqlParameter("@InTime",SqlDbType.DateTime),
                                    new SqlParameter("@Quantity",SqlDbType.Int),
                                    new SqlParameter("@UnitPrice",SqlDbType.Decimal),
                                    new SqlParameter("@PartId",SqlDbType.Int)
                                 };
                pars[0].Value = Num;
                pars[1].Value = PartName;
                pars[2].Value = FromDepartId;
                pars[3].Value = InTimeText;
                pars[4].Value = Quantity;
                pars[5].Value = UnitPrice;
                pars[6].Value = pId;
                DbHelperSQL.ExecuteSql(sbUpdate.ToString(), pars);
            }
            Response.Write("<script language=\"javascript\">opener.location.href=opener.location.href;opener=null;window.close();</script>");

        }
    }
}