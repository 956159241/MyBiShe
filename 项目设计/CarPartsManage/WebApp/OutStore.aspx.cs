using RL.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public partial class OutStore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //数据源
                List<Info> obj = new List<Info>();
                Info myinfo;
                int Num = Convert.ToInt32(Session["quantity"]);
                for (int i = 1; i <= Num; i++)
                {
                    myinfo = new Info();
                    myinfo.OutNum = "";
                    myinfo.PNum = "";
                    myinfo.PName = "";
                    myinfo.OutQuantity = "";
                    myinfo.NeedMerchant = "";
                    obj.Add(myinfo);
                }
                Repeater1.DataSource = obj;
                Repeater1.DataBind();
            }
        }

        /// <summary>
        /// 批量添加保存key-value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutStore_Click(object sender, EventArgs e)
        {
            //循环插入数据并记录操作日志
            foreach (RepeaterItem Item in Repeater1.Items)
            {
                TextBox tbOutNum = (TextBox)Item.FindControl("txtOutNum");
                string OutNum = tbOutNum.Text.ToString();

                TextBox tbPNum = (TextBox)Item.FindControl("txtPNum");
                string PNum = Convert.ToString(tbPNum.Text);

                TextBox tbPName = (TextBox)Item.FindControl("txtPName");
                string PName = tbPName.Text.ToString();

                TextBox tbNeedMerchant = (TextBox)Item.FindControl("txtNeedMerchant");
                string NeedMerchant = Convert.ToString(tbNeedMerchant.Text);
                
                TextBox tbOutQuantity = (TextBox)Item.FindControl("txtOutQuantity");
                int OutQuantity = Convert.ToInt32(tbOutQuantity.Text);

                if (OutNum != " ")
                {
                    string sql = "insert into OutStore values(@OutNum,@PNum,@PName,@OutQuantity,@NeedMerchant,@OutTime);";
                    SqlParameter[] pms = { 
                                        new SqlParameter("@OutNum",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PNum",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@OutQuantity",SqlDbType.Int),
                                        new SqlParameter("@NeedMerchant",SqlDbType.NVarChar,50),
                                        new SqlParameter("@OutTime",SqlDbType.DateTime),
                                     };
                    pms[0].Value = OutNum;
                    pms[1].Value = PNum;
                    pms[2].Value = PName;
                    pms[4].Value = NeedMerchant;
                    pms[3].Value = OutQuantity;
                    pms[5].Value = DateTime.Now;
                    DbHelperSQL.ExecuteSql(sql, pms);

                    //记录操作日志
                    //获取用户登录的Id，并获取登录用户的相关信息，记录操作日志
                    int id = Convert.ToInt32(Session["Id"]);
                    DateTime date = Convert.ToDateTime(Session["LoginTime"]);
                    if (id == 0)
                    {
                        id = 2;
                    }
                    string sqlUsers = "Select UserName,Department from Users where UserId = @id;";
                    SqlParameter[] pms1 = { 
                                                new SqlParameter("@id",SqlDbType.Int),
                                             };
                    pms1[0].Value = id;
                    DataSet ds = DbHelperSQL.Query(sqlUsers, pms1);
                    //string type = ddlType.SelectedValue;
                    //插入日志列表
                    string sqlInsertLog = "insert into Log(Operator,Department,Type,Details,Time,IP)" +
                        " Values(@Operator,@Department,'出库',@Details,getDate(),@IP)";
                    SqlParameter[] pmsInsertLog = { 
                                                  new SqlParameter("@Operator",SqlDbType.NVarChar,50),
                                                  new SqlParameter("@Department",SqlDbType.NVarChar,50),
                                                  //new SqlParameter("@Type",SqlDbType.NVarChar,50),
                                                  new SqlParameter("@Details",SqlDbType.NVarChar,50),
                                                  new SqlParameter("@IP",SqlDbType.NVarChar,50)
                                                  };
                    pmsInsertLog[0].Value = ds.Tables[0].Rows[0]["UserName"];
                    pmsInsertLog[1].Value = ds.Tables[0].Rows[0]["Department"];
                    pmsInsertLog[2].Value = PNum + PName + "出库" + OutQuantity + "件至" + NeedMerchant;
                    pmsInsertLog[3].Value = GetIP();
                    DbHelperSQL.ExecuteSql(sqlInsertLog, pmsInsertLog);

                    //更改需求商的是否发货状态
                    string sqlUpdate = "update NeedMerchant set IsDeliver = 'True' where Num = @NeedNum;";
                    SqlParameter[] pars = { 
                                                new SqlParameter("@NeedNum",SqlDbType.NVarChar,50)
                                             };
                    pars[0].Value = NeedMerchant;
                    DbHelperSQL.ExecuteSql(sqlUpdate, pars);

                    //入库操作,改变数量和单价
                    //string sqlUpdate = "update CarParts set Quantity = Quantity + @InQuantity , UnitPrice = @UnitPrice where Num = @PNum";
                    string sqlUpdateC = "update CarParts set Quantity = Quantity - @Quantity where Num = @PNum;";
                    //DbHelperSQL.ExecuteSql(sqlUpdateC);
                    SqlParameter[] pmsUpdateC = { 
                                                new SqlParameter("@Quantity",SqlDbType.Int),
                                                new SqlParameter("@PNum",SqlDbType.NVarChar,50)
                                             };
                    pmsUpdateC[0].Value = OutQuantity;
                    pmsUpdateC[1].Value = PNum;
                    DbHelperSQL.ExecuteSql(sqlUpdateC, pmsUpdateC);
                }              
            }

            Response.Redirect("Index2.aspx");

        }
        public string GetIP()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "127.0.0.1";
            }
            return result;
        }
    }
}