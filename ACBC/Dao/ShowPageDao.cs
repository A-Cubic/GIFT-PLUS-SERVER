using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.ACBC.Framework.Database;
using System.Data;
using ACBC.Buss;

namespace ACBC.Dao
{
    public class ShowPageDao
    {
        public List<SimpleShowPageList> SimpleShowPageBanner()
        {
            List<SimpleShowPageList> list = new List<SimpleShowPageList>();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(ShowPageDaoSqls.SELECT_T_BUSS_SHOW_PAGE_BANNER, "T").Tables[0];
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

        public List<SimpleShowPageList> SimpleShowPageMenu()
        {
            List<SimpleShowPageList> list = new List<SimpleShowPageList>();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(ShowPageDaoSqls.SELECT_T_BUSS_SHOW_PAGE_MENU, "T").Tables[0];
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

        public SimpleShowPageList SimpleShowPageButton()
        {
            SimpleShowPageList simpleShowPageItem = new SimpleShowPageList();
            DataTable dt = DatabaseOperationWeb.ExecuteSelectDS(ShowPageDaoSqls.SELECT_T_BUSS_SHOW_PAGE_BUTTON, "T").Tables[0];
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
            + " WHERE FLAG='1' AND TYPE='1'  "
            + " ORDER BY SORT ASC ";

        public const string SELECT_T_BUSS_SHOW_PAGE_MENU = ""
            + "SELECT NAME,URL,IMG,SORT "
            + " FROM T_BUSS_SHOW_PAGE "
            + " WHERE FLAG='1' AND TYPE='2'  "
            + " ORDER BY SORT ASC ";

        public const string SELECT_T_BUSS_SHOW_PAGE_BUTTON = ""
           + "SELECT NAME,URL,IMG,SORT "
           + " FROM T_BUSS_SHOW_PAGE "
           + " WHERE FLAG='1' AND TYPE='3'  "
           + " ORDER BY SORT ASC ";
    }
}
