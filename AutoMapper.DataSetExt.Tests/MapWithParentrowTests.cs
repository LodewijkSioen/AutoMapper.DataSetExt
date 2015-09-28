using AutoMapper.DataSetExt.Tests.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;

namespace AutoMapper.DataSetExt.Tests
{
    public class MapWithParentrowTests : IDisposable
    {
        public MapWithParentrowTests()
        {
            Mapper.CreateMap<DataRow, Person>()
                .FromDataSet()
                .ForMember(m => m.Addresses, m => m.Ignore());//otherwise StackOverflow!

            Mapper.CreateMap<DataRow, Address>()
                .FromDataSet()
                .ForMember(m => m.Inhabitant, m => m.WithParentRelation("AddressToPerson"));
        }

        public void MapToAddress()
        {
            var set = Factory.CreateDataSet();

            var addresses = Mapper.Map<IList<Address>>(set.Tables["Addresses"].Rows);

            addresses.Count.ShouldBe(3);
            var address1 = addresses[0];
            address1.StreetName.ShouldBe("JosAddress1");
            address1.Inhabitant.Name.ShouldBe("Jos");
            address1.Inhabitant.Addresses.ShouldBeNull();
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
