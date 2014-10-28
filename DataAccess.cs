            //Data dependent routing - Query single shard
            using (SqlConnection sqlConnection = rangeShardMap.OpenConnectionForKey(1, Configuration.GetShardConnectionString()))
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "Select * from Customer";
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (sqlDataReader.Read())
                {
                    string firstName = sqlDataReader["FirstName"] as string;
                };
            }

            //Multi Shard query
            using (MultiShardConnection multiShardConnection = new MultiShardConnection(rangeShardMap.GetShards(), Configuration.GetShardConnectionString()))
            {
                using (MultiShardCommand multiShardCommand = multiShardConnection.CreateCommand())
                {

                    multiShardCommand.CommandText = "Select * from Customer";
                    multiShardCommand.ExecutionOptions = MultiShardExecutionOptions.IncludeShardNameColumn;

                    MultiShardDataReader multiShardDataReader = multiShardCommand.ExecuteReader();
                    while (multiShardDataReader.Read())
                    {
                        string firstName = multiShardDataReader["FirstName"] as string;
                    };
                }
            }