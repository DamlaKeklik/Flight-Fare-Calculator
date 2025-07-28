using Bimser.CSP.DataSource.Api.Base;
using Bimser.CSP.DataSource.Api.Models;
using Bimser.Framework.AspNetCore.Mvc.Attributes;
using Bimser.Framework.Dependency;
using Bimser.Synergy.Entities.DataSource;
using Bimser.Synergy.Entities.DataSource.Providers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Bimser.Framework.Domain.Option;
using Bimser.Framework.Domain.Option.Filters;
using Bimser.Framework.Domain.Option.Pagination;
using Bimser.Framework.Domain.Option.Sorts;
using Bimser.Synergy.Entities.DataSource.Providers.Database;
using System.Collections.Generic;
using System.IO;

namespace damlaucus.DataSources 
{
    [Route("apps/damlaucus/latest/api/DataSource/[action]")]
    [Route("apps/damlaucus/{v:int:min(1)}/api/DataSource/[action]")]
    [Route("api/DataSource/[action]")]
    [Produces ("application/json")]
    public class DataSourceController : BaseDataSourceController 
    {

        #region [.ctor]

        public DataSourceController (IIocManager iocManager, string authorization = "", string bimserEncryptedData = "", string bimserLanguage = "") : base (iocManager, authorization, bimserEncryptedData, bimserLanguage) 
        {

        }

        #endregion

        ///Actions
        [HttpPost]
[AcceptVerbs("POST")]
[ActionName("Flow1_ProcessItems")]
[NoRequestHeaders]
[NoResponseHeaders]
public async Task<object> Flow1_ProcessItems_Action([FromBody] Flow1_ProcessItemsRequest request)
{
    return await Flow1_ProcessItems(request);
}

[HttpPost]
[AcceptVerbs("POST")]
[ActionName("sehirler")]
[NoRequestHeaders]
[NoResponseHeaders]
public async Task<object> sehirler_Action([FromBody] sehirlerRequest request)
{
    return await sehirler(request);
}

[HttpPost]
[AcceptVerbs("POST")]
[ActionName("standartucretler")]
[NoRequestHeaders]
[NoResponseHeaders]
public async Task<object> standartucretler_Action([FromBody] standartucretlerRequest request)
{
    return await standartucretler(request);
}

        ///Handles
        internal async Task<DataSourceResponse<object>> Flow1_ProcessItems(Flow1_ProcessItemsRequest request)
{
    string queryName = "Flow1_ProcessItems";
    var queryDetails = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), @$"DataSource/Queries/{queryName}/Content.json"));

    JObject queryDetailObject = JObject.Parse(queryDetails);
    var queryContent = JObject.FromObject(queryDetailObject?["content"]);

    string connectionId = queryContent.Value<string>("connectionId");

    DataSourceConnectionObject connectionObj = await DataSourceApi.GetConnectionObject(GetContextData(), HttpClientOptions, connectionId);
    CurrentUserInfo currentUserInfo = await DataSourceApi.GetCurrentUserInfo(GetContextData(), HttpClientOptions);

    IProvider provider = null;
    DataSourceResponse response = null;
    try
    {
        provider = DataSourceApi.CreateProvider(queryContent, connectionObj, currentUserInfo);

        string requestContent = queryContent["properties"].ToString();

    var executeRequest = provider.MergeParameters(queryName, request,  requestContent);

        if (string.IsNullOrWhiteSpace(queryContent["structure"]?["returnType"]?.ToString()))
        {
            queryContent["structure"]["returnType"] = 0;
        }
	
        var structure = queryContent["structure"]?.ToObject<DatabaseStructure>();

        response = await provider.ExecuteAsync(executeRequest, structure);
    }
    finally
    {
        if (provider != null)
        {
            provider.Dispose();
            IocManager.Release(provider);
        }
    }
    return response;
}

internal async Task<DataSourceResponse<object>> sehirler(sehirlerRequest request)
{
    string queryName = "sehirler";
    var queryDetails = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), @$"DataSource/Queries/{queryName}/Content.json"));

    JObject queryDetailObject = JObject.Parse(queryDetails);
    var queryContent = JObject.FromObject(queryDetailObject?["content"]);

    string connectionId = queryContent.Value<string>("connectionId");

    DataSourceConnectionObject connectionObj = await DataSourceApi.GetConnectionObject(GetContextData(), HttpClientOptions, connectionId);
    CurrentUserInfo currentUserInfo = await DataSourceApi.GetCurrentUserInfo(GetContextData(), HttpClientOptions);

    IProvider provider = null;
    DataSourceResponse response = null;
    try
    {
        provider = DataSourceApi.CreateProvider(queryContent, connectionObj, currentUserInfo);

        string requestContent = queryContent["properties"].ToString();

    var executeRequest = provider.MergeParameters(queryName, request,  requestContent);

        if (string.IsNullOrWhiteSpace(queryContent["structure"]?["returnType"]?.ToString()))
        {
            queryContent["structure"]["returnType"] = 0;
        }
	
        var structure = queryContent["structure"]?.ToObject<DatabaseStructure>();

        response = await provider.ExecuteAsync(executeRequest, structure);
    }
    finally
    {
        if (provider != null)
        {
            provider.Dispose();
            IocManager.Release(provider);
        }
    }
    return response;
}

internal async Task<DataSourceResponse<object>> standartucretler(standartucretlerRequest request)
{
    string queryName = "standartucretler";
    var queryDetails = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), @$"DataSource/Queries/{queryName}/Content.json"));

    JObject queryDetailObject = JObject.Parse(queryDetails);
    var queryContent = JObject.FromObject(queryDetailObject?["content"]);

    string connectionId = queryContent.Value<string>("connectionId");

    DataSourceConnectionObject connectionObj = await DataSourceApi.GetConnectionObject(GetContextData(), HttpClientOptions, connectionId);
    CurrentUserInfo currentUserInfo = await DataSourceApi.GetCurrentUserInfo(GetContextData(), HttpClientOptions);

    IProvider provider = null;
    DataSourceResponse response = null;
    try
    {
        provider = DataSourceApi.CreateProvider(queryContent, connectionObj, currentUserInfo);

        string requestContent = queryContent["properties"].ToString();

    var executeRequest = provider.MergeParameters(queryName, request,  requestContent);

        if (string.IsNullOrWhiteSpace(queryContent["structure"]?["returnType"]?.ToString()))
        {
            queryContent["structure"]["returnType"] = 0;
        }
	
        var structure = queryContent["structure"]?.ToObject<DatabaseStructure>();

        response = await provider.ExecuteAsync(executeRequest, structure);
    }
    finally
    {
        if (provider != null)
        {
            provider.Dispose();
            IocManager.Release(provider);
        }
    }
    return response;
}

        ///Iterators
        [NonAction]
public IEnumerable<Dictionary<string, object>> Flow1_ProcessItemsIterator(Flow1_ProcessItemsRequest request)
{
    int pagingSkip = 0;
    int pagingTake = 100;
    int pageIndex = 0;
    var filters = new List<IFilter>();
    var sorts = new List<ISort>();

    bool endOfSource = false;
    while (!endOfSource)
    {
        if (request.LoadOptions != null)
        {
            filters = request.LoadOptions.Filters;
            sorts = request.LoadOptions.Sorts;
            if (pageIndex > 0 && request.LoadOptions.Pagination != null)
            {
                pagingSkip = request.LoadOptions.Pagination.Skip + request.LoadOptions.Pagination.Take;
                pagingTake = request.LoadOptions.Pagination.Take;
            }
        }

        request.LoadOptions = new DataSourceLoadOptions(filters, sorts, new Pagination(pagingSkip, pagingTake));

        var response = Flow1_ProcessItems(request).Result;
        endOfSource = response.IsEndOfSource;
        if (response.Success && response.Result != null)
        {
            if (response.Result is List<Dictionary<string, object>> list)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
            else if (response.Result is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    var dataItem = item.ToObject<Dictionary<string, object>>();
                    yield return dataItem;
                }
            }
        }
        else
        {
            endOfSource = true;
        }

        pageIndex++;
    }
}

[NonAction]
public IEnumerable<Dictionary<string, object>> sehirlerIterator(sehirlerRequest request)
{
    int pagingSkip = 0;
    int pagingTake = 100;
    int pageIndex = 0;
    var filters = new List<IFilter>();
    var sorts = new List<ISort>();

    bool endOfSource = false;
    while (!endOfSource)
    {
        if (request.LoadOptions != null)
        {
            filters = request.LoadOptions.Filters;
            sorts = request.LoadOptions.Sorts;
            if (pageIndex > 0 && request.LoadOptions.Pagination != null)
            {
                pagingSkip = request.LoadOptions.Pagination.Skip + request.LoadOptions.Pagination.Take;
                pagingTake = request.LoadOptions.Pagination.Take;
            }
        }

        request.LoadOptions = new DataSourceLoadOptions(filters, sorts, new Pagination(pagingSkip, pagingTake));

        var response = sehirler(request).Result;
        endOfSource = response.IsEndOfSource;
        if (response.Success && response.Result != null)
        {
            if (response.Result is List<Dictionary<string, object>> list)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
            else if (response.Result is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    var dataItem = item.ToObject<Dictionary<string, object>>();
                    yield return dataItem;
                }
            }
        }
        else
        {
            endOfSource = true;
        }

        pageIndex++;
    }
}

[NonAction]
public IEnumerable<Dictionary<string, object>> standartucretlerIterator(standartucretlerRequest request)
{
    int pagingSkip = 0;
    int pagingTake = 100;
    int pageIndex = 0;
    var filters = new List<IFilter>();
    var sorts = new List<ISort>();

    bool endOfSource = false;
    while (!endOfSource)
    {
        if (request.LoadOptions != null)
        {
            filters = request.LoadOptions.Filters;
            sorts = request.LoadOptions.Sorts;
            if (pageIndex > 0 && request.LoadOptions.Pagination != null)
            {
                pagingSkip = request.LoadOptions.Pagination.Skip + request.LoadOptions.Pagination.Take;
                pagingTake = request.LoadOptions.Pagination.Take;
            }
        }

        request.LoadOptions = new DataSourceLoadOptions(filters, sorts, new Pagination(pagingSkip, pagingTake));

        var response = standartucretler(request).Result;
        endOfSource = response.IsEndOfSource;
        if (response.Success && response.Result != null)
        {
            if (response.Result is List<Dictionary<string, object>> list)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
            else if (response.Result is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    var dataItem = item.ToObject<Dictionary<string, object>>();
                    yield return dataItem;
                }
            }
        }
        else
        {
            endOfSource = true;
        }

        pageIndex++;
    }
}

    }
}