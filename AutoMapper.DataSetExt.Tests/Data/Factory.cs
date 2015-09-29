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
            var personToAddressColumn = persons.Columns.Add("DomicileId", typeof (int));

            var addresses = set.Tables.Add("Addresses");
            var addressIdColumn = addresses.Columns.Add("Id", typeof(int));
            var addressToPersonColumn = addresses.Columns.Add("PersonId", typeof(int));
            addresses.Columns.Add("StreetName", typeof(string));

            set.Relations.Add("PersonProperties", personIdColumn, addressToPersonColumn);
            set.Relations.Add("AddressDomicile", personToAddressColumn, addressIdColumn, false);

            persons.Rows.Add(1, "Jos", 1);
            persons.Rows.Add(2, "André", 1);

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
        public IList<Address> Properties { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public Person Owner { get; set; }
        public IList<Person>  Inhabitants { get; set; }
    }
}
