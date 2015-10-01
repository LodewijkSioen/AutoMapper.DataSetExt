using System.Data;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class PersonDataTable : DataTable
    {
        public PersonDataTable()
            : base("Persons")
        {
            Columns.Add("Id", typeof(int));
            Columns.Add("col_name", typeof(string));
            Columns.Add("DomicileId", typeof(int));
        }
    }
}