using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.ACBC.Framework.Database;
using System.Data;
using ACBC.Buss;
using System.Text;

namespace ACBC.Dao
{
    public class ShowPageDao
    {
        public List<SimpleShowPageList> SimpleShowPageBanner(string equipmentId)
        {
            List<SimpleShowPageList> list = new List<SimpleShowPageList>();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(ShowPageDaoSqls.SELECT_T_BUSS_SHOW_PAGE_BANNER, equipmentId);
            string select = stringBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            if (dt.Rows.Count>0)
            {
                for (int i=0;i< dt.Rows.Count;i++)
                {
                    SimpleShowPageList simpleShowPageItem = new SimpleShowPageList();
                    simpleShowPageItem.name = dt.Rows[i]["NAME"].ToString();
                    simpleShowPageItem.url= dt.Rows[i]["url"].ToString();
                    simpleShowPageItem.img = dt.Rows[i]["img"].ToString();
                    list.Add(simpleShowPageItem);
                }
            }
            return list;
        }

        public List<SimpleShowPageList> SimpleShowPageMenu(string equipmentId)
        {
            List<SimpleShowPageList> list = new List<SimpleShowPageList>();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(ShowPageDaoSqls.SELECT_T_BUSS_SHOW_PAGE_MENU, equipmentId);
            string select = stringBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleShowPageList simpleShowPageItem = new SimpleShowPageList();
                    simpleShowPageItem.name = dt.Rows[i]["NAME"].ToString();
                    simpleShowPageItem.url = dt.Rows[i]["url"].ToString();
                    simpleShowPageItem.img = dt.Rows[i]["img"].ToString();
                    list.Add(simpleShowPageItem);
                }
            }
            return list;
        }

        public SimpleShowPageList SimpleShowPageButton(string equipmentId)
        {
            SimpleShowPageList simpleShowPageItem = new SimpleShowPageList();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(ShowPageDaoSqls.SELECT_T_BUSS_SHOW_PAGE_BUTTON, equipmentId);
            string select = stringBuilder.ToString();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(select, "T").Tables[0];
            if (dt.Rows.Count > 0)
            {
                simpleShowPageItem.name = dt.Rows[0]["NAME"].ToString();
                simpleShowPageItem.url = dt.Rows[0]["url"].ToString();
                simpleShowPageItem.img = dt.Rows[0]["img"].ToString();
            }
            return simpleShowPageItem;
        }
    }

    public class ShowPageDaoSqls
    {
        public const string SELECT_T_BUSS_SHOW_PAGE_BANNER = ""
            + "SELECT NAME,URL,IMG,SORT "
            + " FROM T_BUSS_SHOW_PAGE "
            + " WHERE FLAG='1' AND TYPE='1' AND EQUIPMENTID='{0}' "
            + " ORDER BY SORT ASC ";

        public const string SELECT_T_BUSS_SHOW_PAGE_MENU = ""
            + "SELECT NAME,URL,IMG,SORT "
            + " FROM T_BUSS_SHOW_PAGE "
            + " WHERE FLAG='1' AND TYPE='2' AND  EQUIPMENTID='{0}' "
            + " ORDER BY SORT ASC ";

        public const string SELECT_T_BUSS_SHOW_PAGE_BUTTON = ""
           + "SELECT NAME,URL,IMG,SORT "
           + " FROM T_BUSS_SHOW_PAGE "
           + " WHERE FLAG='1' AND TYPE='3' AND EQUIPMENTID='{0}' "
           + " ORDER BY SORT ASC ";
    }
}
