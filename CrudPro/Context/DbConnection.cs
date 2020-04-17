using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrudPro.Context
{
    public class DbConnection : IDbConnection
    {
        protected string DatabaseId { get; }
        protected string CollectionId { get; set; }



        private DocumentClient _client;
        private readonly string _endpointUrl;
        private readonly string _authKey;
        private readonly Random _random = new Random();
        private const string _partitionKey = "ZipCode";
        private readonly ILogger<IDbConnection> _logger;
        public DbConnection(IConfiguration config, ILogger<IDbConnection> logger)
        {
            DatabaseId = config.GetValue<string>("CosmosDbConnection:DatabaseId");
            _endpointUrl = config.GetValue<string>("CosmosDbConnection:AccountURL");
            _authKey = config.GetValue<string>("CosmosDbConnection:AuthKey");
            _logger = logger;
        }

        public async Task<bool> CreateDatabase(string name)
        {
            try
            {
                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = name });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateCollection(string dbName, string name)
        {
            try
            {
                await _client.CreateDocumentCollectionIfNotExistsAsync
                 (UriFactory.CreateDatabaseUri(dbName), new DocumentCollection { Id = name });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateDocument(string dbName, string name, User userInfo)
        {
            try
            {
                userInfo.Id = "d9e51c1e-1474-41d1-8f32-96deedd8f36a";
                await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(dbName, name), userInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }
        

        public async Task<UserInfo> UpsertUserAsync(UserInfo user)
        {
            ResourceResponse<Document> response = null;
            try
            {
                response = null;//await _client.UpsertDocumentAsync(_collectionUri, user);
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

            return (dynamic)response.Resource;
        }

        public async Task<UserInfo> DeleteUserAsync(string dbName, string name, string id)
        {
            try
            {
                var collectionUri = UriFactory.CreateDocumentUri(dbName, name, id);

                var result = await _client.DeleteDocumentAsync(collectionUri);

                return (dynamic)result.Resource;
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DocumentClient InitializeAsync(string collectionId)
        {
            CollectionId = collectionId;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Gateway,
                ConnectionProtocol = Protocol.Https
            };

            if (_client == null)
                _client = new DocumentClient(
                    new Uri(_endpointUrl), _authKey, connectionPolicy);

            return _client;
        }
    }
}
