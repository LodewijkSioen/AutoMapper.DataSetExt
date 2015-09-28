using AutoMapper.DataSetExt.Tests.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AutoMapper.DataSetExt.Tests
{
    public class RowWithChildrowTests : IDisposable
    {
        public RowWithChildrowTests()
        {
            var personmap = Mapper.CreateMap<DataRow, Person>();
            personmap.ForAllMembers(m => m.ResolveUsing<DataRowResolver>());
            personmap.ForMember(m => m.Addresses, m => m.ResolveUsing(new ChildRowResolver("AddressToPerson")));

            var addressmap = Mapper.CreateMap<DataRow, Address>();
            addressmap.ForAllMembers(m => m.ResolveUsing<DataRowResolver>());
            addressmap.ForMember(m => m.Inhabitant, m => m.Ignore());//otherwise StackOverflow!
        }

        public void MapToPerson()
        {
            var set = Factory.CreateDataSet();            

            var persons = Mapper.Map<IEnumerable<Person>>(set.Tables["Persons"].Rows);

            persons.Count().ShouldBe(2);
            var jos = persons.ElementAt(0);
            jos.Name.ShouldBe("Jos");
            jos.Addresses.Count().ShouldBe(2);
            jos.Addresses.ElementAt(0).StreetName.ShouldBe("JosAddress1");
            jos.Addresses.ElementAt(1).StreetName.ShouldBe("JosAddress2");
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
