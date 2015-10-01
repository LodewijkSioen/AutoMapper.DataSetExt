namespace AutoMapper.DataSetExt.Tests.Data
{
    public static class Factory
    {
        public static DemoDataSet CreateDataSet()
        {
            var set = new DemoDataSet();

            set.Tables["Persons"].Rows.Add(1, "Jos", 1);
            set.Tables["Persons"].Rows.Add(2, "André", 1);

            set.Tables["Addresses"].Rows.Add(1, 1, "JosAddress1");
            set.Tables["Addresses"].Rows.Add(2, 1, "JosAddress2");
            set.Tables["Addresses"].Rows.Add(3, 2, "AndréAddress1");

            return set;
        }
    }
}
