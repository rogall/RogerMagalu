using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Settings
{
    public class MongoDBSettings : IMongoDBSettings
    {        
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ClientesCollectionName { get; set; }
        public string ClientesProdutosCollectionName { get; set; }
    }

    public interface IMongoDBSettings
    {        
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ClientesCollectionName { get; set; }
        string ClientesProdutosCollectionName { get; set; }
    }
}
