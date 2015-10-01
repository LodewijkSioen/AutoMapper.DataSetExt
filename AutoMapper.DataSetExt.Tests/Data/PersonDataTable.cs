using System;
using System.Data;
using System.Linq;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class PersonDataTable : DataTable
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

        protected override Type GetRowType()
        {
            return typeof (PersonDataRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new PersonDataRow(builder);
        }
    }

    public class PersonDataRow: DataRow
    {
        public PersonDataRow(DataRowBuilder builder) : base(builder)
        {
        }

        public AddressDataRow[] Properties => GetChildRows("PersonProperties").Cast<AddressDataRow>().ToArray();
    }
}