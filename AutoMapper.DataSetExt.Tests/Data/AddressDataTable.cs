using System.Data;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class AddressDataTable : DataTable
    {
        public AddressDataTable()
            : base("Addresses")
        {
            Columns.Add("Id", typeof(int));
            Columns.Add("PersonId", typeof(int));
            Columns.Add("StreetName", typeof(string));
        }
    }
}