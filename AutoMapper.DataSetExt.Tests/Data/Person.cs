using System.Collections.Generic;

namespace AutoMapper.DataSetExt.Tests.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Address> Properties { get; set; }
    }
}