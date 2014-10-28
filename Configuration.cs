    public static class Configuration
    {
        private static string _esUserId = "";
        private static string _esPassword = "";
        private static string _esServerName = "";
        private static string _esDataBase = "";

        public static string EsUserId
        {
            get
            {
                if (String.IsNullOrEmpty(_esUserId))
                    _esUserId = ConfigurationManager.AppSettings["EsUserId"];
                return _esUserId;
            }
        }
        public static string EsPassword
        {
            get
            {
                if (String.IsNullOrEmpty(_esPassword))
                    _esPassword = ConfigurationManager.AppSettings["EsPassword"];
                return _esPassword;
            }
        }

        public static string EsServerName
        {
            get
            {
                if (String.IsNullOrEmpty(_esServerName))
                    _esServerName = ConfigurationManager.AppSettings["EsServername"];
                return _esServerName;
            }
        }

        public static string EsDataBase
        {
            get
            {
                if (String.IsNullOrEmpty(_esDataBase))
                    _esDataBase = ConfigurationManager.AppSettings["EsDataBase"];
                return _esDataBase;
            }
        }

        internal static string GetGsmConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                InitialCatalog = Configuration.EsDataBase,
                DataSource = Configuration.EsServerName,
                UserID = Configuration.EsUserId,
                Password = Configuration.EsPassword,
                IntegratedSecurity = false,
                ApplicationName = "GlobalyScalable",
                ConnectTimeout = 30
            }.ToString();
        }

        internal static string GetShardConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                UserID = Configuration.EsUserId,
                Password = Configuration.EsPassword,
                IntegratedSecurity = false,
                ApplicationName = "GlobalyScalable",
                ConnectTimeout = 30,
            }.ToString();
        }
    }