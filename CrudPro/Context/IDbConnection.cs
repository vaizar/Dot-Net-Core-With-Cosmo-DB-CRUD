using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace CrudPro.Context
{
    public interface IDbConnection
    {
        DocumentClient InitializeAsync(string collectionId);
    }
}
