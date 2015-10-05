using System;
using System.Collections.Generic;
using System.Data;
using AutoMapper;
using DataSetExperiments.Tests.Data;
using Shouldly;

namespace DataSetExperiments.Tests
{
    public class AutoMapperTests : IDisposable
    {
        public AutoMapperTests()
        {
            var set = new DemoDataSet();

            Mapper.CreateMap<DataRow, Person>()
                .FromDataSet()
                .ForMember(m => m.Name, m => m.MapFromColumn(set.Persons.Name))
                .ForMember(m => m.Properties, m => m.WithChildRelation(set.PersonProperties));

            Mapper.CreateMap<DataRow, Address>()
                .FromDataSet()
                .ForMember(m => m.Owner, m => m.WithParentRelation(set.PersonProperties))
                .ForMember(m => m.Inhabitants, m => m.WithParentRelation(set.AddressDomicile));
        }

        public void ParentToChildRelation()
        {
            var set = Factory.CreateDataSet();

            var perons = Mapper.Map<IList<Person>>(set.Persons.Rows);

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

        public void ChildToParentRelation()
        {
            var set = Factory.CreateDataSet();

            var addresses = Mapper.Map<IList<Address>>(set.Addresses.Rows);

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
