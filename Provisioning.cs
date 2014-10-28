            //Create GSM - Global Shard Map
            bool smmExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
                Configuration.GetGsmConnectionString(),
                ShardMapManagerLoadPolicy.Lazy,
                out _shardMapManager
            );

            if (!smmExists)
            {
                ShardMapManagerFactory.CreateSqlShardMapManager(Configuration.GetGsmConnectionString());
                _shardMapManager = ShardMapManagerFactory.GetSqlShardMapManager(Configuration.GetGsmConnectionString(), ShardMapManagerLoadPolicy.Lazy);
            }

            //Create Shard Map
            string shardMapName = "CustomerSharding";
            RangeShardMap<int> rangeShardMap;
            bool shardMapExists = _shardMapManager.TryGetRangeShardMap(shardMapName, out rangeShardMap);
            if (!shardMapExists)
            {
                rangeShardMap = _shardMapManager.CreateRangeShardMap<int>(shardMapName);
            }

            //Create Schema Info
            if (_shardMapManager.GetSchemaInfoCollection().Count() == 0)
            {
                SchemaInfo schemaInfo = new SchemaInfo();
                schemaInfo.Add(new ShardedTableInfo("Customer", "Id"));
                _shardMapManager.GetSchemaInfoCollection().Add(shardMapName, schemaInfo);
            }

            //Create - Register Shard(s)
            Shard shard01 = null;
            Shard shard02 = null;
            if (!rangeShardMap.GetShards().Any())
            {
                ShardLocation shardLocation = new ShardLocation(Configuration.EsServerName, String.Format("{0}_Shard0{1}", Configuration.EsDataBase, 1));
                shard01 = rangeShardMap.CreateShard(shardLocation);

                ShardLocation shardLocation2 = new ShardLocation(Configuration.EsServerName, String.Format("{0}_Shard0{1}", Configuration.EsDataBase, 2));
                shard02 = rangeShardMap.CreateShard(shardLocation2);

                RangeMapping<int> rangeMapping = rangeShardMap.CreateRangeMapping(new Range<int>(0, 5), shard01);
                rangeMapping = rangeShardMap.CreateRangeMapping(new Range<int>(5, 10), shard02);
            }