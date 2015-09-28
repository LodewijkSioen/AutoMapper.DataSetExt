using System;
using System.Data;

namespace AutoMapper.DataSetExt
{
    public class DataRowResolver : IValueResolver
    {
        public ResolutionResult Resolve(ResolutionResult source)
        {
            return source.New(Convert.ChangeType((source.Context.SourceValue as DataRow)[source.Context.MemberName], source.Context.DestinationType));
        }
    }

    public class ChildRowResolver : IValueResolver
    {
        private string _relationName;

        public ChildRowResolver(string relationName)
        {
            _relationName = relationName;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            var row = source.Context.SourceValue as DataRow;

            var childRows = row.GetChildRows(_relationName);

            return source.New(Mapper.Map(childRows, typeof(DataRow[]), source.Context.DestinationType));
        }
    }

    public class ParentRowResolver : IValueResolver
    {
        private string _relationName;

        public ParentRowResolver(string relationName)
        {
            _relationName = relationName;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            var row = source.Context.SourceValue as DataRow;

            var parentRow = row.GetParentRow(_relationName);

            return source.New(Mapper.Map(parentRow, typeof(DataRow), source.Context.DestinationType));
        }
    }
}
