using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACBC.Common;
using ACBC.Dao;

namespace ACBC.Buss
{
    public class ShowPageBuss : IBuss
    {
        public ApiType GetApiType()
        {
            return ApiType.ShowPageApi;
        }

        public SimpleShowPageItem Do_SimpleShowPage(BaseApi baseApi)
        {
            ShowPageDao showPageDao = new ShowPageDao();
            SimpleShowPageItem simpleShowPageItem = new SimpleShowPageItem();
            simpleShowPageItem.banners = showPageDao.SimpleShowPageBanner();
            simpleShowPageItem.menu = showPageDao.SimpleShowPageMenu();
            simpleShowPageItem.button = showPageDao.SimpleShowPageButton();
            return simpleShowPageItem;
        }
    }
}
