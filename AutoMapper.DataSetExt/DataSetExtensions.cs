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
            if (Table.DataSet == null)
            {
                throw new NotSupportedException("Relations only work if DataTables are part of a DataSet.");
            }

            var set = Table.DataSet as TDataSet;
            if (set == null)
            {
                throw new NotSupportedException($"This relation is only available if the table of type {Table.GetType()} is part of a DataSet of type {typeof(TDataSet)}. The current DataSet is of type {Table.DataSet.GetType()}");
            }

            var relation = relationGetter(set);

            return GetChildRows(relation).Cast<TDataRow>();
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
