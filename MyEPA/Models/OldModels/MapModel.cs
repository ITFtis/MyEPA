using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class MapModel
    {
        public String Xpos { get; set; }
        public String Ypos { get; set; }
        public String FindCode(string location)
        {
            String Code = String.Empty;
            switch (location)
            {
                case "環保署":
                    Code = "000";
                    break;
                case "基隆市":
                    Code = "001";
                    break;
                case "臺北市":
                    Code = "002";
                    break;
                case "新北市":
                    Code = "003";
                    break;
                case "桃園市":
                    Code = "004";
                    break;
                case "新竹縣":
                    Code = "005";
                    break;
                case "新竹市":
                    Code = "006";
                    break;
                case "苗栗縣":
                    Code = "007";
                    break;
                case "臺中市":
                    Code = "008";
                    break;
                case "南投縣":
                    Code = "009";
                    break;
                case "彰化縣":
                    Code = "010";
                    break;
                case "雲林縣":
                    Code = "011";
                    break;
                case "嘉義縣":
                    Code = "012";
                    break;
                case "嘉義市":
                    Code = "013";
                    break;
                case "臺南市":
                    Code = "014";
                    break;
                case "高雄市":
                    Code = "015";
                    break;
                case "屏東縣":
                    Code = "016";
                    break;
                case "臺東縣":
                    Code = "017";
                    break;
                case "花蓮縣":
                    Code = "018";
                    break;
                case "宜蘭縣":
                    Code = "019";
                    break;
                case "澎湖縣":
                    Code = "020";
                    break;
                case "金門縣":
                    Code = "021";
                    break;
                case "連江縣":
                    Code = "022";
                    break;
                default:
                    Code = null;
                    break;
            }
            return Code;
        }
        public MapModel FindGPS(string location)
        {
            switch (location)
            {
                case "環保署":
                    Xpos = "25.039673"; Ypos = "121.507812";
                    break;
                case "基隆市":
                    Xpos = "25.1302681"; Ypos = "121.7359251";
                    break;
                case "臺北市":
                    Xpos = "25.0375417"; Ypos = "121.5622494";
                    break;
                case "新北市":
                    Xpos = "25.0216685"; Ypos = "121.4480526";
                    break;
                case "桃園市":
                    Xpos = "24.9904319"; Ypos = "121.2971787";
                    break;
                case "新竹縣":
                    Xpos = "24.8269196"; Ypos = "121.011165";
                    break;
                case "新竹市":
                    Xpos = "24.8062337"; Ypos = "120.9683981";
                    break;
                case "苗栗縣":
                    Xpos = "24.5609258"; Ypos = "120.7989517";
                    break;
                case "臺中市":
                    Xpos = "24.1618586"; Ypos = "120.366716";
                    break;
                case "南投縣":
                    Xpos = "23.9025996"; Ypos = "120.6883162";
                    break;
                case "彰化縣":
                    Xpos = "24.075515"; Ypos = "120.5425027";
                    break;     
                case "雲林縣":
                    Xpos = "23.699157"; Ypos = "120.5241447";
                    break;
                case "嘉義縣":
                    Xpos = "23.4586628"; Ypos = "120.2907856";
                    break;
                case "嘉義市":
                    Xpos = "23.4812869"; Ypos = "120.4492166";
                    break;
                case "臺南市":
                    Xpos = "22.9854712"; Ypos = "120.1800782";
                    break;
                case "高雄市":
                    Xpos = "22.6355497"; Ypos = "120.17";
                    break;
                case "屏東縣":
                    Xpos = "22.6830792"; Ypos = "120.4857815";
                    break;
                case "臺東縣":
                    Xpos = "22.7554656"; Ypos = "121.1483485";
                    break;
                case "花蓮縣":
                    Xpos = "23.991331"; Ypos = "121.6176327";
                    break;
                case "宜蘭縣":
                    Xpos = "24.7307143"; Ypos = "121.7609331";
                    break;
                case "澎湖縣":
                    Xpos = "23.569999"; Ypos = "119.5641927";
                    break;
                case "連江縣":
                    Xpos = "26.1578637"; Ypos = "119.949601";
                    break;
                case "金門縣":
                    Xpos = "24.4367833"; Ypos = "118.3164973";
                    break;
                default:
                    Xpos = "25.02";Ypos = "121.32";
                    break;
            }
            MapModel Place = new MapModel();
            Place.Xpos = Xpos;
            Place.Ypos = Ypos;
            return Place;
        }
    }
}