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
        public const string ROUTE_PX = "/api/config";
        public const int REDIS_DB = 11;
        public static BaseBuss BUSS = new BaseBuss();
        public static Dictionary<string, Dictionary<string, ConfigGroup>> ConfigList;

        public static void StartUp()
        {
            if (DatabaseOperationWeb.TYPE == null)
            {
                DatabaseOperationWeb.TYPE = new DBManager();
            }

            //new ConfigBuss().GetConfigAll();

        }

        public static string Redis
        {
            get
            {
                return Environment.GetEnvironmentVariable("Redis");
            }
        }

        public static string DBUrl
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBUrl");
            }
        }

        public static string DBUser
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBUser");
            }
        }

        public static string DBPort
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBPort");
            }
        }

        public static string DBPassword
        {
            get
            {
                return Environment.GetEnvironmentVariable("DBPassword");
            }
        }
    }
}
