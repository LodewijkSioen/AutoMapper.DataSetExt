using AutoMapper.DataSetExt.Tests.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AutoMapper.DataSetExt.Tests
{
    public class RowWithParentrowTests : IDisposable
    {
        public RowWithParentrowTests()
        {
            var personmap = Mapper.CreateMap<DataRow, Person>();
            personmap.ForAllMembers(m => m.ResolveUsing<DataRowResolver>());
            personmap.ForMember(m => m.Addresses, m => m.Ignore());//otherwise StackOverflow!

            var addressmap = Mapper.CreateMap<DataRow, Address>();
            addressmap.ForAllMembers(m => m.ResolveUsing<DataRowResolver>());
            addressmap.ForMember(m => m.Inhabitant, m => m.ResolveUsing(new ParentRowResolver("AddressToPerson")));
        }

        public void MapToAddress()
        {
            var set = Factory.CreateDataSet();

            var addresses = Mapper.Map<IEnumerable<Address>>(set.Tables["Addresses"].Rows);

            addresses.Count().ShouldBe(3);
            var address1 = addresses.ElementAt(0);
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
