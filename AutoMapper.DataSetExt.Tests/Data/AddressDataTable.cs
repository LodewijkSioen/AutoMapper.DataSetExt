using System.Collections.Generic;
using System.Data;

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

        public IEnumerable<PersonDataRow> Inhabitants => GetParentRows<DemoDataSet, PersonDataRow>(s => s.AddressDomicile);

        public PersonDataRow Owner => GetParentRow<DemoDataSet, PersonDataRow>(s => s.PersonProperties);
    }
}