using System.Collections.Generic;
using System.Data;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public static class Factory
    {
        public static DataSet CreateDataSet()
        {
            var set = new DataSet("PersonsAndAddresses");

            var persons = set.Tables.Add("Persons");
            var personIdColumn = persons.Columns.Add("Id", typeof(int));
            persons.Columns.Add("Name", typeof(string));

            var addresses = set.Tables.Add("Addresses");
            addresses.Columns.Add("Id", typeof(int));
            var addressToPersonColumn = addresses.Columns.Add("PersonId", typeof(int));
            addresses.Columns.Add("StreetName", typeof(string));

            set.Relations.Add("AddressToPerson", personIdColumn, addressToPersonColumn);

            persons.Rows.Add(1, "Jos");
            persons.Rows.Add(2, "André");

            addresses.Rows.Add(1, 1, "JosAddress1");
            addresses.Rows.Add(2, 1, "JosAddress2");
            addresses.Rows.Add(3, 2, "AndréAddress1");

            return set;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Address> Addresses { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public Person Inhabitant { get; set; }
    }
}
