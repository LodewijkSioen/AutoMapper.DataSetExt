using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AutoMapper.DataSetExt
{
    public abstract class DataRow<T> : DataRow
        where T :  DataTable
    {
        public new T Table => base.Table as T;

        protected DataRow(DataRowBuilder builder) : base(builder)
        {
        }

        public IEnumerable<TDataRow> GetChildRows<TDataSet, TDataRow>(Func<TDataSet, DataRelation> relationGetter)
            where TDataSet : DataSet
            where TDataRow : DataRow
        {
            var set = CastToCorrectDataSetType<TDataSet>();

            var relation = relationGetter(set);

            return GetChildRows(relation).Cast<TDataRow>();
        }

        public IEnumerable<TDataRow> GetParentRows<TDataSet, TDataRow>(Func<TDataSet, DataRelation> relationGetter)
            where TDataSet : DataSet
            where TDataRow : DataRow
        {
            var set = CastToCorrectDataSetType<TDataSet>();

            var relation = relationGetter(set);

            return GetParentRows(relation).Cast<TDataRow>();
        }

        public TDataRow GetParentRow<TDataSet, TDataRow>(Func<TDataSet, DataRelation> relationGetter)
            where TDataSet : DataSet
            where TDataRow : DataRow
        {
            var set = CastToCorrectDataSetType<TDataSet>();

            var relation = relationGetter(set);

            return GetParentRow(relation) as TDataRow;
        }

        private TDataSet CastToCorrectDataSetType<TDataSet>()
            where TDataSet : DataSet
        {
            if (Table == null)
            {
                throw new NotSupportedException($"This DataRow of type {GetType()} is not part of a DataTable. Relations only work if the DataRow is part of a DataTable which belongs to a DataSet.");
            }

            if (Table.DataSet == null)
            {
                throw new NotSupportedException($"This DataRow of type {GetType()} is part of a DataTable with name {Table.TableName} and of type {Table.GetType()} that is not part of a DataSet. Relations only work if DataTables are part of a DataSet.");
            }

            var set = Table.DataSet as TDataSet;
            if (set == null)
            {
                throw new NotSupportedException($"This relation is only available if the table of type {Table.GetType()} is part of a DataSet of type {typeof(TDataSet)}. The current DataSet is of type {Table.DataSet.GetType()}");
            }
            return set;
        }
    }

    public abstract class DataTable<T> : DataTable, IEnumerable<T> 
        where T : DataRow
    {
        protected DataTable(string tableName)
            : base(tableName)
        {
        }

        protected override Type GetRowType() => typeof(T);

        public IEnumerator<T> GetEnumerator()
        {
            return Rows.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
