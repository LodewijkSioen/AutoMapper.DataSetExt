using System;
using System.Collections.Generic;
using System.Data;
using AutoMapper.DataSetExt.Tests.Data;
using Shouldly;

namespace AutoMapper.DataSetExt.Tests
{
    public class MapWithParentChildRelationTests : IDisposable
    {
        public MapWithParentChildRelationTests()
        {
            Mapper.CreateMap<DataRow, Person>()
                .FromDataSet()
                .ForMember(m => m.Addresses, m => m.WithChildRelation("AddressToPerson"));

            Mapper.CreateMap<DataRow, Address>()
                .FromDataSet()
                .ForMember(m => m.Inhabitant, m => m.WithParentRelation("AddressToPerson"));
        }

        public void MapParentToChildrenRelation()
        {
            var set = Factory.CreateDataSet();

            var perons = Mapper.Map<IList<Person>>(set.Tables["Persons"].Rows);

            perons.Count.ShouldBe(2);
            var jos = perons[0];
            jos.Id.ShouldBe(1);
            jos.Name.ShouldBe("Jos");
            jos.Addresses[0].StreetName.ShouldBe("JosAddress1");
            jos.Addresses[0].Inhabitant.ShouldBe(jos);
            jos.Addresses[1].StreetName.ShouldBe("JosAddress2");
            jos.Addresses[1].Inhabitant.ShouldBe(jos);
            var andré = perons[1];
            andré.Id.ShouldBe(2);
            andré.Name.ShouldBe("André");
            andré.Addresses[0].StreetName.ShouldBe("AndréAddress1");
            andré.Addresses[0].Inhabitant.ShouldBe(andré);
        }

        public void MapChildToParentRelation()
        {
            var set = Factory.CreateDataSet();

            var addresses = Mapper.Map<IList<Address>>(set.Tables["Addresses"].Rows);

            addresses.Count.ShouldBe(3);
            var address1 = addresses[0];
            address1.StreetName.ShouldBe("JosAddress1");
            address1.Inhabitant.Name.ShouldBe("Jos");
            address1.Inhabitant.Addresses.Count.ShouldBe(2);
            address1.Inhabitant.Addresses.ShouldContain(address1);
            var address2 = addresses[1];
            address2.StreetName.ShouldBe("JosAddress2");
            address2.Inhabitant.Name.ShouldBe("Jos");
            address2.Inhabitant.Addresses.Count.ShouldBe(2);
            address2.Inhabitant.Addresses.ShouldContain(address2);
            var address3 = addresses[2];
            address3.StreetName.ShouldBe("AndréAddress1");
            address3.Inhabitant.Name.ShouldBe("André");
            address3.Inhabitant.Addresses.Count.ShouldBe(1);
            address3.Inhabitant.Addresses.ShouldContain(address3);
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
