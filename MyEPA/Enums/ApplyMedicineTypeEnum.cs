using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ApplyMedicineTypeEnum
    {
        [Description("殺菌劑(公升)")]
        Fungicide = 1,
        [Description("殺蟲劑(公斤)")]
        Insecticide = 2,
        [Description("漂白水(公升)")]
        BleachWater = 3
    }
}