namespace MmaSolution.Core.Database.Tables
{
    public class SysSetting : BaseEntity<int>
    {
        public string SysKey { get; private set; }
        public string SysValue { get; private set; }
    }
}
