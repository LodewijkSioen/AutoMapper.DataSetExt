using System.Collections.Generic;
using System.Data;

namespace DataSetExperiments.Tests.Data
{
    public class PersonDataTable : DataTable<PersonDataRow>
    {
        public PersonDataTable()
            : base("Persons")
        {
            Columns.Add(Id);
            Columns.Add(Name);
            Columns.Add(DomicileId);
        }

        public DataColumn Id { get; } = new DataColumn("Id", typeof(int));
        public DataColumn Name { get; } = new DataColumn("col_name", typeof(string));
        public DataColumn DomicileId { get; } = new DataColumn("DomicileId", typeof(int));

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder) => new PersonDataRow(builder);
    }

    public class PersonDataRow: DataRow<PersonDataTable>
    {
        public PersonDataRow(DataRowBuilder builder) : base(builder)
        {
        }

        public int Id => this.Field<int>(Table.Id);
        public string Name => this.Field<string>(Table.Name);
        public IEnumerable<AddressDataRow> Properties => GetChildRows<DemoDataSet, AddressDataRow>(s => s.PersonProperties);
    }
}