using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum DisinfectorNameEnum
    {
        [Description("噴霧機")]
        Sprayer,
        [Description("消毒機(器)")]
        Disinfector,
        [Description("熱煙霧機")]
        HotSmokeSachine,
        [Description("高壓清洗機")]
        PressureWasher,
        [Description("車載式高壓噴霧機")]
        SprayerCAR,
        [Description("高壓噴霧機")]
        SprayeSrHI,
        [Description("超低容量噴霧機")]
        SprayeSrLO,
        [Description("車載式煙霧機")]
        SMOK,
        [Description("其他")]
        Other,
    }
}