using System.Data;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class DemoDataSet : DataSet
    {
        public DemoDataSet()
        {
            Tables.Add(new PersonDataTable());
            Tables.Add(new AddressDataTable());

            Relations.Add("PersonProperties", Tables["Persons"].Columns["Id"], Tables["Addresses"].Columns["PersonId"], false);
            Relations.Add("AddressDomicile", Tables["Persons"].Columns["DomicileId"], Tables["Addresses"].Columns["Id"], false);
        }
    }
}