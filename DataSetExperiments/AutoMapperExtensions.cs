using System;
using System.Data;
using AutoMapper;
using AutoMapper.Internal;

namespace DataSetExperiments
{
    public static class Extensions
    {
        public static IMappingExpression<DataRow, TDestination> FromDataSet<TDestination>(this IMappingExpression<DataRow, TDestination> mappingExpression)
        {
            mappingExpression.ForAllMembers(m => m.ResolveUsing<DataRowResolver>());
            return mappingExpression;
        }

        public static IMemberConfigurationExpression<DataRow> WithChildRelation(
            this IMemberConfigurationExpression<DataRow> memberConfigurationExpression,
            string relationName)
        {
            memberConfigurationExpression.ResolveUsing(new ChildRowResolver(relationName));
            return memberConfigurationExpression;
        }

        public static IMemberConfigurationExpression<DataRow> WithChildRelation(
            this IMemberConfigurationExpression<DataRow> memberConfigurationExpression,
            DataRelation relation)
        {
            memberConfigurationExpression.ExplicitExpansion();
            memberConfigurationExpression.ResolveUsing(new ChildRowResolver(relation));
            return memberConfigurationExpression;
        }

        public static IMemberConfigurationExpression<DataRow> WithParentRelation(
            this IMemberConfigurationExpression<DataRow> memberConfigurationExpression,
            string relationName)
        {
            memberConfigurationExpression.ResolveUsing(new ParentRowResolver(relationName));
            return memberConfigurationExpression;
        }

        public static IMemberConfigurationExpression<DataRow> WithParentRelation(
            this IMemberConfigurationExpression<DataRow> memberConfigurationExpression,
            DataRelation relation)
        {
            memberConfigurationExpression.ResolveUsing(new ParentRowResolver(relation));
            return memberConfigurationExpression;
        }

        public static IMemberConfigurationExpression<DataRow> MapFromColumn(
            this IMemberConfigurationExpression<DataRow> memberConfigurationExpression,
            string columnName)
        {
            memberConfigurationExpression.MapFrom(r => r[columnName]);
            return memberConfigurationExpression;
        }

        public static IMemberConfigurationExpression<DataRow> MapFromColumn(
            this IMemberConfigurationExpression<DataRow> memberConfigurationExpression,
            DataColumn column)
        {
            memberConfigurationExpression.MapFrom(r => r[column.ColumnName]);
            return memberConfigurationExpression;
        }

        internal static DataRow ExtractDataRow(this ResolutionResult source)
        {
            var row = source.Context.SourceValue as DataRow;
            if (row == null)
            {
                throw new ArgumentException($"The SourceValue for dealing with a Dataset must be a DataRow. Current type is a '{source.Context.SourceType}'");
            }

            return row;
        }
    }

    public class DataRowResolver : IValueResolver
    {
        public ResolutionResult Resolve(ResolutionResult source)
        {
            var row = source.ExtractDataRow();

            return source.New(row[source.Context.MemberName]);
        }
    }

    public class ChildRowResolver : IValueResolver
    {
        private readonly string _relationName;

        public ChildRowResolver(string relationName)
        {
            _relationName = relationName;
        }

        public ChildRowResolver(DataRelation relation)
        {
            _relationName = relation.RelationName;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            var row = source.ExtractDataRow();

            var childRows = row.GetChildRows(_relationName);
            
            return source.New(childRows, source.Context.DestinationType);
        }
    }

    public class ParentRowResolver : IValueResolver
    {
        private readonly string _relationName;

        public ParentRowResolver(string relationName)
        {
            _relationName = relationName;
        }

        public ParentRowResolver(DataRelation relation)
        {
            _relationName = relation.RelationName;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            var row = source.ExtractDataRow();

            if (source.Context.DestinationType.IsEnumerableType())
            {
                var parentRows = row.GetParentRows(_relationName);
                return source.New(parentRows, source.Context.DestinationType);
            }

            var parentRow = row.GetParentRow(_relationName);
            return source.New(parentRow, source.Context.DestinationType);
        }
    }
}
