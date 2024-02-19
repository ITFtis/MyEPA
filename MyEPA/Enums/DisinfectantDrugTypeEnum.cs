using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum DisinfectantDrugTypeEnum
    {
        /// <summary>
        /// 殺菌劑
        /// </summary>
        [Description("殺菌劑")]
        Fungicide,
        /// <summary>
        /// 殺蟲劑
        /// </summary>
        [Description("殺蟲劑")]
        Insecticide

    }
}