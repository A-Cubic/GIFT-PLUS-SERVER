using Com.ACBC.Framework.Database;
using ACBC.Buss;
using ACBC.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Common
{
    public class Global
    {
        public const string ROUTE_PX = "/api/giftmanage";
        public const int REDIS_DB = 11;
        public static BaseBuss BUSS = new BaseBuss();
        public static Dictionary<string, Dictionary<string, ConfigGroup>> ConfigList;

        public static void StartUp()
        {
            if (DatabaseOperationWeb.TYPE == null)
            {
                DatabaseOperationWeb.TYPE = new DBManager();
            }
        }

        public static string Redis
        {
            get
            {
                return Environment.GetEnvironmentVariable("redis");
            }
        }

        public static string DBUrl
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBUrl");
            }
        }

        public static string DBUser
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBUser");
            }
        }

        public static string DBPort
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBPort");
            }
        }

        public static string DBPassword
        {
            get
            {
                return Environment.GetEnvironmentVariable("MysqlDBPassword");
            }
        }
    }
}
