using CrudPro.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CrudPro.Models;

namespace CrudPro.Context
{
    public class UserInfo : IUserInfo
    {
        private readonly DocumentClient _client;
        private readonly string _accountUrl;
        private readonly string _primarykey;

        public UserInfo(
         IDbConnection connection,
         IConfiguration config)
        {
            _accountUrl = config.GetValue<string>("CosmosDbConnection:AccountURL");
            _primarykey = config.GetValue<string>("CosmosDbConnection:AuthKey");
            _client = new DocumentClient(new Uri(_accountUrl), _primarykey);
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

        public async Task<bool> CreateDocument(string dbName, string name, Models.User userInfo)
        {
            try
            {
                await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(dbName, name), userInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<dynamic> GetData(string dbName, string name)
        {
            try
            {

                var result = await _client.ReadDocumentFeedAsync(UriFactory.CreateDocumentCollectionUri(dbName, name),
                    new FeedOptions { MaxItemCount = 10 });

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UserInfo> DeleteUser(string dbName, string name, string id)
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

        public Task<Models.User> UpsertUser(Models.User user)
        {
            throw new NotImplementedException();
        }

        Task<Models.User> IUserInfo.DeleteUser(string dbName, string name, string id)
        {
            throw new NotImplementedException();
        }
    }
}
