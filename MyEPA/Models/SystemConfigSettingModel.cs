namespace MyEPA.Models
{
    public class SystemConfigSettingModel
    {
        [AutoKey]
        public int FunctionId { get; set; }
        public string Value { get; set; }
        public string Memo { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}