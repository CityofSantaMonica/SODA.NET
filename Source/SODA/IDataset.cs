using System.Collections.Generic;

using SODA.Models;

namespace SODA
{
    public interface IDataset
    {
        IEnumerable<Row> Search(string search);
        IEnumerable<Row> Query(SoqlQuery query);
        IEnumerable<Row> Query(string query);
        Row GetRow(string rowId);
    }
}
