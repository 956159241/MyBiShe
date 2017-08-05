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
    public partial class InStore : System.Web.UI.Page
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
                    myinfo.InNum = "";
                    myinfo.PNum = "";
                    myinfo.PName = "";
                    myinfo.UnitPrice = "";
                    myinfo.InQuantity = "";
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
        protected void InStore_Click(object sender, EventArgs e)
        {
            //循环插入数据并记录操作日志
            foreach (RepeaterItem Item in Repeater1.Items)
            {
                //TextBox tbKeys = (TextBox)Item.FindControl("txtPName");
                //TextBox tbValuse = (TextBox)Item.FindControl("txtPNum");
                //Response.Write("<b>keys:</b>" + tbKeys.Text + "   <b>Values:</b>" + tbValuse.Text + "   <b>是否有效:</b>");
                //Response.Write("<br/>");
                //TextBox InNum1 = (TextBox)Item.FindControl("txtInNum");
                //string InNum = Convert.ToString(InNum1);
                //Console.Write(InNum);
                //string InNum = Item.FindControl("txtInNum").ToString();
                TextBox tbInNum = (TextBox)Item.FindControl("txtInNum");
                string InNum = tbInNum.Text.ToString();
                TextBox tbPNum = (TextBox)Item.FindControl("txtPNum");
                string PNum = Convert.ToString(tbPNum.Text);
                TextBox tbPName = (TextBox)Item.FindControl("txtPName");
                string PName = tbPName.Text.ToString();
                TextBox tbUnitPrice = (TextBox)Item.FindControl("txtUnitPrice");
                Decimal  UnitPrice = Convert.ToDecimal(tbUnitPrice.Text);
                TextBox tbInQuantity = (TextBox)Item.FindControl("txtInQuantity");
                int InQuantity = Convert.ToInt32(tbInQuantity.Text);
                
                if (InNum != " ")
                {
                    string sql = "insert into InStore values(@InNum,@PNum,@PName,@UnitPrice,@InQuantity,@InTime);";
                    SqlParameter[] pms = { 
                                        new SqlParameter("@InNum",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PNum",SqlDbType.NVarChar,50),
                                        new SqlParameter("@PName",SqlDbType.NVarChar,50),
                                        new SqlParameter("@UnitPrice",SqlDbType.Decimal),
                                        new SqlParameter("@InQuantity",SqlDbType.Int),
                                        new SqlParameter("@InTime",SqlDbType.DateTime),
                                     };
                    pms[0].Value = InNum;
                    pms[1].Value = PNum;
                    pms[2].Value = PName;
                    pms[3].Value = UnitPrice;
                    pms[4].Value = InQuantity;
                    pms[5].Value = DateTime.Now;
                    DbHelperSQL.ExecuteSql(sql, pms);
                    
                }
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
                    " Values(@Operator,@Department,'入库',@Details,getDate(),@IP)";
                SqlParameter[] pmsInsertLog = { 
                                                  new SqlParameter("@Operator",SqlDbType.NVarChar,50),
                                                  new SqlParameter("@Department",SqlDbType.NVarChar,50),
                                                  //new SqlParameter("@Type",SqlDbType.NVarChar,50),
                                                  new SqlParameter("@Details",SqlDbType.NVarChar,50),
                                                  new SqlParameter("@IP",SqlDbType.NVarChar,50)
                                                  };
                pmsInsertLog[0].Value = ds.Tables[0].Rows[0]["UserName"];
                pmsInsertLog[1].Value = ds.Tables[0].Rows[0]["Department"];
                pmsInsertLog[2].Value = PNum + PName + "入库" + InQuantity + "件";
                pmsInsertLog[3].Value = GetIP();
                DbHelperSQL.ExecuteSql(sqlInsertLog, pmsInsertLog);

                //入库操作,改变数量和单价
                //string sqlUpdate = "update CarParts set Quantity = Quantity + @InQuantity , UnitPrice = @UnitPrice where Num = @PNum";
                string sqlUpdateC = "update CarParts set Quantity = Quantity + @Quantity , " + 
                    "UnitPrice = (Quantity * UnitPrice + @Quantity * @UnitPrice)/ (Quantity + @Quantity) where Num = @PNum;";
                //DbHelperSQL.ExecuteSql(sqlUpdateC);
                SqlParameter[] pmsUpdateC = { 
                                                new SqlParameter("@Quantity",SqlDbType.Int),
                                                new SqlParameter("@UnitPrice",SqlDbType.Decimal),
                                                new SqlParameter("@PNum",SqlDbType.NVarChar,50)
                                             };
                pmsUpdateC[0].Value = InQuantity;
                pmsUpdateC[1].Value = UnitPrice;
                pmsUpdateC[2].Value = PNum;
                DbHelperSQL.ExecuteSql(sqlUpdateC, pmsUpdateC);
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

    /// <summary>
    /// 信息类
    /// </summary>
    public class Info
    {
        private string _inNum;
        private string _pNum;
        private string _unitPrice;
        private string _inQuantity;
        private string _pName;
        private string _outNum;
        private string _outQuantity;
        private string _needMerchant;

        /// <summary>
        /// 入库单号
        /// </summary>
        public string InNum
        {
            get
            {
                return this._inNum;
            }
            set
            {
                this._inNum = value;
            }

        }
        /// <summary>
        /// 配件单号
        /// </summary>
        public string PNum
        {
            get
            {
                return this._pNum;
            }
            set
            {
                this._pNum = value;
            }

        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public string PName
        {
            get
            {
                return this._pName;
            }
            set
            {
                this._pName = value;
            }

        }
        /// <summary>
        /// 单价
        /// </summary>
        public string UnitPrice
        {
            get
            {
                return this._unitPrice;
            }
            set
            {
                this._unitPrice = value;
            }

        }
        /// <summary>
        /// 出库数量
        /// </summary>
        public string InQuantity
        {
            get
            {
                return this._inQuantity;
            }
            set
            {
                this._inQuantity = value;
            }

        }
        public string OutNum
        {
            get
            {
                return this._outNum;
            }
            set
            {
                this._outNum = value;
            }
        }
        public string OutQuantity
        {
            get
            {
                return this._outQuantity;
            }
            set
            {
                this._outQuantity = value;
            }

        }
        public string NeedMerchant
        {
            get
            {
                return this._needMerchant;
            }
            set
            {
                this._needMerchant = value;
            }

        }

        
    }
}
