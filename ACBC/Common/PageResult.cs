using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class PageResult
    {
        public object item;
        public List<Object> list;
        public Page pagination;
    }
    public class Page
    {
        public int current;
        public int total;
        public int pageSize;

        public Page(int current, int pageSize)
        {
            this.current = current;
            this.pageSize = pageSize;
        }
    }

    public class MsgResult
    {
        public string msg;
        public int type=0;//0失败，1成功
    }
}
