using System.Collections.Generic;

namespace DataSetExperiments.Tests.Data
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public Person Owner { get; set; }
        public IList<Person>  Inhabitants { get; set; }
    }
}