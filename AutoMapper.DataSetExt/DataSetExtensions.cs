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
