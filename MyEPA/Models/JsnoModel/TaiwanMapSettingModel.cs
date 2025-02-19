using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.JsnoModel
{
    public class TaiwanMapTransformModel
    {
        public List<double> scale { get; set; }
        public List<double> translate { get; set; }
    }

    public class TaiwanMapPropertiesModel
    {
        public string TOWN_ID { get; set; }
        public string TOWN { get; set; }
        public string COUNTY_ID { get; set; }
        public string COUNTY { get; set; }
        public int H_CNT { get; set; }
        public int P_CNT { get; set; }
        public int M_CNT { get; set; }
        public int F_CNT { get; set; }
        public string INFO_TIME { get; set; }
        public string site_id { get; set; }
        public decimal DisinfectorCount { get; set; }
        public decimal DisinfectantLiquidAmount { get; set; }
        public int DisinfectantSolidAmount { get; set; }
        public int UserCount { get; set; }
        public int VehicleCount { get; set; }

        public int PestCount { get; set; }
        public int DumpCount { get; set; }
        public int ToiletCount { get; set; }

        public int VolunteerCount { get; set; }
        public int GarbageLandfillCount { get; set; }

        public int DefendCount { get; set; }
    }

    public class TaiwanMapGeometryModel
    {
        public List<List<object>> arcs { get; set; }
        public string type { get; set; }
        public TaiwanMapPropertiesModel properties { get; set; }
    }

    public class TwMercatorModel
    {
        public string type { get; set; }
        public List<TaiwanMapGeometryModel> geometries { get; set; }
    }

    public class TwMercatorObjectModel
    {
        public TwMercatorModel tw_mercator { get; set; }
    }

    public class TaiwanMapSettingModel
    {
        public string type { get; set; }
        public List<List<List<int>>> arcs { get; set; }
        public TaiwanMapTransformModel transform { get; set; }
        public TwMercatorObjectModel objects { get; set; }
    }
}