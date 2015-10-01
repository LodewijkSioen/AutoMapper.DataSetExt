using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class AddressDataTable : DataTable
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

        protected override Type GetRowType()
        {
            return typeof(AddressDataRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new AddressDataRow(builder);
        }
    }

    public class AddressDataRow : DataRow
    {
        public AddressDataRow(DataRowBuilder builder) : base(builder)
        {
        }
        public PersonDataRow[] Inhabitants => GetChildRows("AddressDomicile").Cast<PersonDataRow>().ToArray();
        public PersonDataRow Owner => GetParentRow("PersonProperties") as PersonDataRow;

    }
}