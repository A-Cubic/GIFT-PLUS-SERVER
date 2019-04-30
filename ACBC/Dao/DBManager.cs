using Com.ACBC.Framework.Database;
using System;

namespace ACBC.Dao
{
    public class DBManager : IType
    {
        private DBType dbt;
        private string str = "";

        public DBManager()
        {
            this.str = "Server=" + Common.Global.DBUrl
                     + ";Port=" + Common.Global.DBPort
                     + ";Database=gift;Uid=" + Common.Global.DBUser
                     + ";Pwd=" + Common.Global.DBPassword
                     + ";CharSet=utf8mb4; SslMode =none;";
            Console.Write(this.str);
            this.dbt = DBType.Mysql;
        }

        public DBManager(DBType d, string s)
        {
            this.dbt = d;
            this.str = s;
        }

        public DBType getDBType()
        {
            return dbt;
        }

        public string getConnString()
        {
            return str;
        }

        public void setConnString(string s)
        {
            this.str = s;
        }
    }
}
