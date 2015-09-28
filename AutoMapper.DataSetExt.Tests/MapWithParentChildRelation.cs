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

        public void MapRelation()
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

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
