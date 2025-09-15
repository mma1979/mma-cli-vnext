namespace MmaSolution.Core.Models.SysSetting
{
    public partial class SysSettingReadModel
    {
        public string SysKey { get; set; }
        public string SysValue { get; set; }
        public int Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}