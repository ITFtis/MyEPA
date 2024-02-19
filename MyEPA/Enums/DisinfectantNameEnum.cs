using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum DisinfectantNameEnum
    {
        /// <summary>
        /// 第滅寧
        /// </summary>
        [Description("第滅寧")]
        DELTAMETHRIN,
        /// <summary>
        /// 亞特松
        /// </summary>
        [Description("亞特松")]
        PirimiphosMethyl,
        /// <summary>
        /// 漂白水
        /// </summary>
        [Description("漂白水")]
        Bleach,
        /// <summary>
        ///凱普多乳劑
        /// </summary>
        [Description("凱普多乳劑")]
        CapdoEmulsion,
        /// <summary>
        /// 賽滅寧
        /// </summary>
        [Description("賽滅寧")]
        Cypermethrin,
        /// <summary>
        /// 愛清淨
        /// </summary>
        [Description("愛清淨")]
        LoveClean,
        /// <summary>
        /// 撲滅松乳劑
        /// </summary>
        [Description("撲滅松乳劑")]
        Fenitrothion,
        /// <summary>
        /// 酷滅寧乳劑
        /// </summary>
        [Description("酷滅寧乳劑")]
        Kubening,
        [Description("其他")]
        Other,
    }
}