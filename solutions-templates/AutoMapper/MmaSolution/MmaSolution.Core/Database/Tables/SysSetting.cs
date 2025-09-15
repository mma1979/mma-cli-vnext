namespace MmaSolution.Core.Database.Tables
{
    public class SysSetting : BaseEntity<int>
    {
        public string SysKey { get; private set; }
        public Dictionary<string, object> SysValue { get; private set; }
        public string Environment { get; private set; }
    }
}
