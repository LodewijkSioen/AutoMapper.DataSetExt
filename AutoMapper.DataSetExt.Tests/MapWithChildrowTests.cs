using AutoMapper.DataSetExt.Tests.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;

namespace AutoMapper.DataSetExt.Tests
{
    public class MapWithChildrowTests : IDisposable
    {
        public MapWithChildrowTests()
        {
            Mapper.CreateMap<DataRow, Person>()
                .FromDataSet()
                .ForMember(m => m.Addresses, m => m.WithChildRelation("AddressToPerson"));

            Mapper.CreateMap<DataRow, Address>()
                .FromDataSet()
                .ForMember(m => m.Inhabitant, m => m.Ignore());//otherwise StackOverflow!
        }

        public void MapToPerson()
        {
            var set = Factory.CreateDataSet();            

            var persons = Mapper.Map<IList<Person>>(set.Tables["Persons"].Rows);

            persons.Count.ShouldBe(2);
            var jos = persons[0];
            jos.Name.ShouldBe("Jos");
            jos.Addresses.Count.ShouldBe(2);
            jos.Addresses[0].StreetName.ShouldBe("JosAddress1");
            jos.Addresses[0].Inhabitant.ShouldBeNull();
            jos.Addresses[1].StreetName.ShouldBe("JosAddress2");
            jos.Addresses[1].Inhabitant.ShouldBeNull();
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
