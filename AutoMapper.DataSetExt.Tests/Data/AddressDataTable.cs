using System;
using System.Data;
using System.Linq;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class AddressDataTable : DataTable<AddressDataRow>
    {
        public AddressDataTable()
            : base("Addresses")
        {
            Columns.Add(Id);
            Columns.Add(PersonId);
            Columns.Add(StreetName);
        }

        public DataColumn Id { get; }= new DataColumn("Id", typeof(int));
        public DataColumn PersonId { get; }= new DataColumn("PersonId", typeof(int));
        public DataColumn StreetName { get; }= new DataColumn("StreetName", typeof(string));
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder) => new AddressDataRow(builder);
    }

    public class AddressDataRow : DataRow<AddressDataTable>
    {
        public AddressDataRow(DataRowBuilder builder) : base(builder)
        {
        }

        public string StreetName => this.Field<string>(Table.StreetName);

        public PersonDataRow[] Inhabitants => GetParentRows("AddressDomicile").Cast<PersonDataRow>().ToArray();

        public PersonDataRow Owner => GetParentRow("PersonProperties") as PersonDataRow;
    }
}