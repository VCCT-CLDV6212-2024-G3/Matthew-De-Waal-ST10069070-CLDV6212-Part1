
using Azure.Data.Tables;
using Microsoft.Extensions.ObjectPool;
using System.Collections;
using System.Collections.Specialized;

namespace ABCRetail.Models.Tables
{
    public abstract class TableStorageClass : IEnumerable<string[]>
    {
        /// <summary>
        /// Data fields.
        /// </summary>
        private readonly string? connectionString;
        private readonly string? tableName;
        private readonly string[]? columns;

        /// <summary>
        /// Automatic Properties
        /// </summary>
        public string? ConnectionString => connectionString;
        public string? TableName => tableName;

        /// <summary>
        /// Master constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        public TableStorageClass(string? connectionString, string? tableName, string[] columns)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
            this.columns = columns;
        }

        /// <summary>
        /// Adds a new record to the table.
        /// </summary>
        /// <param name="record">The record as a string array that holds the cell values.</param>
        public async void AddRecord(string[] record)
        {
            // Declare and instantiate a TableClient object.
            TableClient client = new TableClient(connectionString, tableName);
            // Get the record that keeps track of the record count.
            var tableEntity = client.GetEntityIfExistsAsync<TableEntity>("RecordCount", "0").Result;
            // Declare and initialize this variable to 1.
            int recordCount = 1;

            // Check if the record exists.
            if(tableEntity.HasValue)
            {
                // Entity exists : Update the record that keeps track of the record count.
                TableEntity? entity = tableEntity.Value; 
                
                recordCount = Convert.ToInt32(entity["Value"]);
                recordCount++;
                entity["Value"] = recordCount.ToString();

                client.UpdateEntityAsync<TableEntity>(entity, entity.ETag);
            }
            else
            {
                // Entity does not exist : Add the record that keeps track of the record count.
                var entity = new TableEntity("RecordCount", "0");
                entity["Value"] = "1";

                client.AddEntityAsync<TableEntity>(entity);
            }

            // Declare and instantiate a Dictionary object.
            Dictionary<string, object> recordEntity = new Dictionary<string, object>();
            recordEntity.Add("PartitionKey", recordCount.ToString());
            recordEntity.Add("RowKey", "0");

            // Iterate through the provided columns.
            for(int i = 0; i < columns?.Length; i++)
            {
                string column = columns[i];
                string value = record[i];

                // Add the column and value properties to the recordEntity object.
                recordEntity.Add(column, value);
            }

            // Add the record to the table.
            client.AddEntity<TableEntity>(new TableEntity(recordEntity));
        }

        /// <summary>
        /// Delete the record from the table.
        /// </summary>
        /// <param name="key"></param>
        public virtual async void DeleteRecord(string key)
        {
            // Declare and instantiate a TableClient object.
            TableClient client = new TableClient(connectionString, tableName);

            // Get the record from the table.
            TableEntity entity = client.GetEntityAsync<TableEntity>(key, "0").Result;
            // Delete the record from the table.
            client?.DeleteEntityAsync(entity);
        }

        public string[] this[int recordIndex]
        {
            get
            {
                TableClient client = new TableClient(connectionString, tableName);
                var entities = client.Query<TableEntity>();
                TableEntity? entity = entities.ElementAt(recordIndex);

                string[] values = new string[columns.Length];
                int count = 0;

                for (int i = 0; i < entity.Keys.Count; i++)
                {
                    if (entity.Keys.ElementAt(i) == columns[i])
                    {
                        values[count] = entity[entity.Keys.ToArray()[i]].ToString();
                        count++;
                    }
                }

                return values;
            }
            set
            {
                TableClient client = new TableClient(connectionString, tableName);
                var entities = client.Query<TableEntity>();
                TableEntity? entity = entities.ElementAt(recordIndex);

                int count = 0;

                for(int i = 0; i < entity.Keys.Count; i++)
                {
                    if(columns.Contains(entity.Keys.ElementAt(i)))
                    {
                        entity[columns[count]] = value[count];
                        count++;
                    }
                }

                client.UpdateEntityAsync<TableEntity>(entity, entity.ETag);
            }
        }

        public string this[int recordIndex, string propertyName]
        {
            get
            {
                TableClient client = new TableClient(connectionString, tableName);
                var entities = client.Query<TableEntity>();
                TableEntity? entity = entities.ElementAt(recordIndex);

                return entity[propertyName].ToString();
            }
            set
            {
                TableClient client = new TableClient(connectionString, tableName);
                var entities = client.Query<TableEntity>();
                TableEntity? entity = entities.ElementAt(recordIndex);

                entity[propertyName] = value;
                client.UpdateEntityAsync<TableEntity>(entity, entity.ETag);
            }
        }

        public int Count
        {
            get
            {
                TableClient client = new TableClient(connectionString, tableName);
                var entities = client.Query<TableEntity>();

                return entities.Count();
            }
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            TableClient client = new TableClient(connectionString, tableName);
            var entities = client.Query<TableEntity>();
            List<string> sColumns = new List<string>(columns);

            foreach (var entity in entities)
            {
                string[] values = new string[columns.Length];

                for (int i = 0; i < entity.Keys.Count; i++)
                {
                    if (columns.Contains(entity.Keys.ElementAt(i)))
                    {
                        values[sColumns.IndexOf(entity.Keys.ElementAt(i))] = entity[entity.Keys.ToArray()[i]].ToString();
                    }
                }

                yield return values;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            TableClient client = new TableClient(connectionString, tableName);
            var entities = client.Query<TableEntity>();
            List<string> sColumns = new List<string>(columns);

            foreach (var entity in entities)
            {
                string[] values = new string[columns.Length];

                for (int i = 0; i < entity.Keys.Count; i++)
                {
                    if (columns.Contains(entity.Keys.ElementAt(i)))
                    {
                        values[sColumns.IndexOf(entity.Keys.ElementAt(i))] = entity[entity.Keys.ToArray()[i]].ToString();
                    }
                }

                yield return values;
            }
        }
    }
}
