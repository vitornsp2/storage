using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using findox.Test.TestObjects;
using Microsoft.AspNetCore.Mvc.Testing;

namespace findox.Test.End2End
{
    public abstract class BaseTestClass
    {
        protected static StorageDb _db;
        protected readonly WebApplicationFactory<Program> _webApplicationFactory;
        protected ObjectsDtoTest _dto;
        protected JsonSerializerOptions _jsonOpts;

         public BaseTestClass(WebApplicationFactory<Program> webApplicationFactory)
        {
            if(_db is null)
            {
                _db = new StorageDb();
                _db.RecycleDb().Wait();
            }

            _webApplicationFactory = webApplicationFactory;
            _dto = new ObjectsDtoTest();
            _jsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
    }
}