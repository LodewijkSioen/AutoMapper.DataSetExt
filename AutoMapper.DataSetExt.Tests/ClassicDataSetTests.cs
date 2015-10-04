using System.Linq;
using AutoMapper.DataSetExt.Tests.Data;
using Shouldly;

namespace AutoMapper.DataSetExt.Tests
{
    public class ClassicDataSetTests
    {
        private readonly DemoDataSet _set;

        public ClassicDataSetTests()
        {
            _set = Factory.CreateDataSet();
        }

        public void ParentToChildRelation()
        {
            _set.Persons.Count().ShouldBe(2);
            var jos = _set.Persons.ElementAt(0);
            jos.Id.ShouldBe(1);
            jos.Name.ShouldBe("Jos");
            jos.Properties.ElementAt(0).StreetName.ShouldBe("JosAddress1");
            jos.Properties.ElementAt(0).Owner.ShouldBe(jos);
            jos.Properties.ElementAt(1).StreetName.ShouldBe("JosAddress2");
            jos.Properties.ElementAt(1).Owner.ShouldBe(jos);
            var andré = _set.Persons.ElementAt(1);
            andré.Id.ShouldBe(2);
            andré.Name.ShouldBe("André");
            andré.Properties.ElementAt(0).StreetName.ShouldBe("AndréAddress1");
            andré.Properties.ElementAt(0).Owner.ShouldBe(andré);
        }

        public void ChildToParentRelation()
        {
            _set.Addresses.Count().ShouldBe(3);
            var address1 = _set.Addresses.ElementAt(0);
            address1.StreetName.ShouldBe("JosAddress1");
            address1.Owner.Name.ShouldBe("Jos");
            address1.Owner.Properties.Count().ShouldBe(2);
            address1.Owner.Properties.ShouldContain(address1);
            address1.Inhabitants.Count().ShouldBe(2);
            var address2 = _set.Addresses.ElementAt(1);
            address2.StreetName.ShouldBe("JosAddress2");
            address2.Owner.Name.ShouldBe("Jos");
            address2.Owner.Properties.Count().ShouldBe(2);
            address2.Owner.Properties.ShouldContain(address2);
            address2.Inhabitants.Count().ShouldBe(0);
            var address3 = _set.Addresses.ElementAt(2);
            address3.StreetName.ShouldBe("AndréAddress1");
            address3.Owner.Name.ShouldBe("André");
            address3.Owner.Properties.Count().ShouldBe(1);
            address3.Owner.Properties.ShouldContain(address3);
            address2.Inhabitants.Count().ShouldBe(0);
        }
    }
}