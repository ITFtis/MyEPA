using EPASchedule.Models.Deds;
using EPASchedule.Models.Epaemis_local;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.Web.UI.WebControls;
using static System.Data.Entity.Infrastructure.Design.Executor;
using log4net;

namespace EPASchedule
{
    internal class APIVehicleImport
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute()
        {
            try
            {
                if (!Do())
                {
                    logger.Error("執行失敗");
                }
                else
                {
                    logger.Info("執行成功");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private bool Do()
        {
            try
            {
                List<AR4_newCarKind> AR4_newCarKind = null;
                List<AR5_newCarKind> AR5_newCarKind = null;

                //步驟1：取DEDS資料
                using (var dbDEDS = new DedsModelContext())
                {
                    AR4_newCarKind = dbDEDS.AR4_newCarKind.ToList();
                    AR5_newCarKind = dbDEDS.AR5_newCarKind.ToList();
                }

                if (AR4_newCarKind.Count() == 0 || AR5_newCarKind.Count() == 0)
                {
                    logger.Error("步驟1：取DEDS資料，AR4_newCarKind或AR5_newCarKind無資料");
                    return false;
                }

                //步驟2匯入Epaemis_local
                using (var dbMyData = new MyData())
                {
                    //(1)刪除暫存表(z_AR4, z_AR5)，Copy至暫存表(z_AR4, z_AR5)
                    string sql = @"
                            TRUNCATE TABLE z_AR4_newCarKind 
                            TRUNCATE TABLE z_AR5_newCarKind 
                            

                        ";

                    foreach (var v in AR4_newCarKind)
                    {
                        string _dataSql = string.Format(@"
                                Insert Into z_AR4_newCarKind(id, updDate, DBID, ZipID, CityName, TownName, DepName, AssetNo, VhlName, VhlCount, VhlKindName, OtherVhlRecRptCarKindID, CarNo, Capacity, HorsePower, UseMemo, BuyYear, IsEpaSpr, CarNow, CanSupportCity, CanSupportEpa, Memo, TWD97_X, TWD97_Y, IsDeleted, DeletedDate, WriteTime)
                                Select {0} AS id, '{1}' AS updDate, '{2}' AS DBID, '{3}' AS ZipID, '{4}' AS CityName, 
                                       '{5}' AS TownName, '{6}' AS DepName, '{7}' AS AssetNo, '{8}' AS VhlName, 
                                       '{9}' AS VhlCount, '{10}' AS VhlKindName, '{11}' AS OtherVhlRecRptCarKindID, 
                                       '{12}' AS CarNo, '{13}' AS Capacity, '{14}' AS HorsePower, '{15}' AS UseMemo, '{16}' AS BuyYear, 
                                       '{17}' AS IsEpaSpr, '{18}' AS CarNow, '{19}' AS CanSupportCity, '{20}' AS CanSupportEpa, '{21}' AS Memo, 
                                       '{22}' AS TWD97_X, '{23}' AS TWD97_Y, '{24}' AS IsDeleted, '{25}' AS DeletedDate, '{26}' AS WriteTime
                            ",
                                v.id,
                                v.updDate,
                                v.DBID,
                                v.ZipID,
                                v.CityName,
                                v.TownName,
                                v.DepName,
                                v.AssetNo,
                                v.VhlName,
                                v.VhlCount,
                                v.VhlKindName,
                                v.OtherVhlRecRptCarKindID,
                                v.CarNo,
                                v.Capacity,
                                v.HorsePower,
                                v.UseMemo,
                                v.BuyYear,
                                v.IsEpaSpr,
                                v.CarNow,
                                v.CanSupportCity,
                                v.CanSupportEpa,
                                v.Memo,
                                v.TWD97_X,
                                v.TWD97_Y,
                                v.IsDeleted,
                                v.DeletedDate,
                                v.WriteTime.ToShortDateString());

                        sql += @"
                            " + _dataSql;
                    }

                    dbMyData.Database.ExecuteSqlCommand(sql);

                    //(2)Copy至暫存表(z_AR4, z_AR5)

                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤 - Do");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return false;
            }

            return true;
        }
    }
}
