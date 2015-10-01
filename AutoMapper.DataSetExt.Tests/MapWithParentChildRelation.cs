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
                .ForMember(m => m.Name, m => m.MapFromColumn("col_name"))
                .ForMember(m => m.Properties, m => m.WithChildRelation("PersonProperties"));

            Mapper.CreateMap<DataRow, Address>()
                .FromDataSet()
                .ForMember(m => m.Owner, m => m.WithParentRelation("PersonProperties"))
                .ForMember(m => m.Inhabitants, m => m.WithParentRelation("AddressDomicile"));
        }

        public void MapParentToChildrenRelation()
        {
            var set = Factory.CreateDataSet();

            var perons = Mapper.Map<IList<Person>>(set.Tables["Persons"].Rows);

            perons.Count.ShouldBe(2);
            var jos = perons[0];
            jos.Id.ShouldBe(1);
            jos.Name.ShouldBe("Jos");
            jos.Properties[0].StreetName.ShouldBe("JosAddress1");
            jos.Properties[0].Owner.ShouldBe(jos);
            jos.Properties[1].StreetName.ShouldBe("JosAddress2");
            jos.Properties[1].Owner.ShouldBe(jos);
            var andré = perons[1];
            andré.Id.ShouldBe(2);
            andré.Name.ShouldBe("André");
            andré.Properties[0].StreetName.ShouldBe("AndréAddress1");
            andré.Properties[0].Owner.ShouldBe(andré);
        }

        public void MapChildToParentRelation()
        {
            var set = Factory.CreateDataSet();

            var addresses = Mapper.Map<IList<Address>>(set.Tables["Addresses"].Rows);

            addresses.Count.ShouldBe(3);
            var address1 = addresses[0];
            address1.StreetName.ShouldBe("JosAddress1");
            address1.Owner.Name.ShouldBe("Jos");
            address1.Owner.Properties.Count.ShouldBe(2);
            address1.Owner.Properties.ShouldContain(address1);
            address1.Inhabitants.Count.ShouldBe(2);
            var address2 = addresses[1];
            address2.StreetName.ShouldBe("JosAddress2");
            address2.Owner.Name.ShouldBe("Jos");
            address2.Owner.Properties.Count.ShouldBe(2);
            address2.Owner.Properties.ShouldContain(address2);
            address2.Inhabitants.Count.ShouldBe(0);
            var address3 = addresses[2];
            address3.StreetName.ShouldBe("AndréAddress1");
            address3.Owner.Name.ShouldBe("André");
            address3.Owner.Properties.Count.ShouldBe(1);
            address3.Owner.Properties.ShouldContain(address3);
            address2.Inhabitants.Count.ShouldBe(0);
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
