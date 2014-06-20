using System;

namespace SODA
{
    public interface ISodaClient
    {
        Dataset GetDataset(string domain, string datasetId);
        TResult Get<TResult>(Uri uri);        
        void Post(Uri uri, string body);
        void Put(Uri uri, string body);
        void Delete(Uri uri);
    }
}
