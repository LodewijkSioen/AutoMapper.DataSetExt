using System.Data;

namespace DataSetExperiments.Tests.Data
{
    public class DemoDataSet : DataSet
    {
        public DemoDataSet()
            :base("DemoDataSet")
        {
            Tables.Add(Persons);
            Tables.Add(Addresses);

            PersonProperties = new DataRelation("PersonProperties", Persons.Id, Addresses.PersonId, false);
            Relations.Add(PersonProperties);

            AddressDomicile = new DataRelation("AddressDomicile", Persons.DomicileId, Addresses.Id, false);
            Relations.Add(AddressDomicile);
        }

        public PersonDataTable Persons { get; } = new PersonDataTable();
        public AddressDataTable Addresses { get; } = new AddressDataTable();

        public DataRelation PersonProperties { get; }
        public DataRelation AddressDomicile { get; }
    }
}