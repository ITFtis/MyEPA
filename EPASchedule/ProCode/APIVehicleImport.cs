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
using System.Security.Cryptography;

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
                    //(false)開發測試，省時間AR4,5匯入時間
                    bool IsGo = true;  //false
                    
                    if (IsGo)
                    {
                        //(1)刪除暫存表(z_AR4_newCarKind, z_AR5_newCarKind)
                        //Copy至暫存表(z_AR4_newCarKind, z_AR5_newCarKind)
                        string sql = @"
                            TRUNCATE TABLE z_AR4_newCarKind 
                            TRUNCATE TABLE z_AR5_newCarKind 
                        ";

                        //暫存表z_AR4_newCarKind 測試(AR4_newCarKind.Take(100))
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

                        //暫存表z_AR5_newCarKind 測試(AR5_newCarKind.Take(100))
                        foreach (var v in AR5_newCarKind)
                        {
                            string _dataSql = string.Format(@"
                                Insert Into z_AR5_newCarKind(id, VhlRecUptDate, VhlRecCmpRecID, CityName, TownName, VhlRecCarNo, VhlRecModel, VhlRecCompany, VhlRecVhlBotCmpID, VhlRecBotOtCountry, VhlRecBotOtManufacturer, VhlRecVhlBdyCmpID, VhlRecBdyOtCountry, VhlRecBotOtManufacturer1, VhlRecPrdDate, VhlRecBuyDate, VhlRecRptCarKindID, VhlRecCapacity, VhlRecCapOtNote, VhlRecGear, VhlRecGearCountF, VhlRecExhaust, VhlRecFuel, VhlRecFuelAdd, VhlRecSeat, VhlRecLoad, VhlRecGrossWeight, VhlRecAdditionItem, VhlRecAdditionItemOtNote, VhlRecBuyCompany, VhlRecRealBuyDate, VhlRecBuyPrice, VhlRecWarrantyDate, VhlRecBuyWayID, VhlRecBuyWayOtNote, VhlRecBuyMoneyFrom, VhlRecBuyMoneyFromOtNote, VhlRecDiscardDate, VhlRecDiscardReason, VhlRecDiscardReasonNote, VhlRecDiscard, VhlRecDiscardMoney, R_Year, R_NewCarNo, R_RenewYear, VhlRecRemark, VhlRecCanSupportEpa, VhlRecCanSupportCity, VhlRecTWD97_X, VhlRecTWD97_Y, VhlRecRegYear, VhlRecCatID, VhlRecUseCondition, WriteTime)
                                Select  {0} AS id, '{1}' AS VhlRecUptDate, '{2}' AS VhlRecCmpRecID, '{3}' AS CityName, '{4}' AS TownName, 
                                        '{5}' AS VhlRecCarNo, '{6}' AS VhlRecModel, '{7}' AS VhlRecCompany, '{8}' AS VhlRecVhlBotCmpID, 
                                        '{9}' AS VhlRecBotOtCountry, '{10}' AS VhlRecBotOtManufacturer, '{11}' AS VhlRecVhlBdyCmpID, 
                                        '{12}' AS VhlRecBdyOtCountry, '{13}' AS VhlRecBotOtManufacturer1, '{14}' AS VhlRecPrdDate, 
                                        '{15}' AS VhlRecBuyDate, '{16}' AS VhlRecRptCarKindID, '{17}' AS VhlRecCapacity, '{18}' AS VhlRecCapOtNote, 
                                        '{19}' AS VhlRecGear, '{20}' AS VhlRecGearCountF, '{21}' AS VhlRecExhaust, '{22}' AS VhlRecFuel, 
                                        '{23}' AS VhlRecFuelAdd, '{24}' AS VhlRecSeat, '{25}' AS VhlRecLoad, '{26}' AS VhlRecGrossWeight, 
                                        '{27}' AS VhlRecAdditionItem, '{28}' AS VhlRecAdditionItemOtNote, '{29}' AS VhlRecBuyCompany, 
                                        '{30}' AS VhlRecRealBuyDate, '{31}' AS VhlRecBuyPrice, '{32}' AS VhlRecWarrantyDate, 
                                        '{33}' AS VhlRecBuyWayID, '{34}' AS VhlRecBuyWayOtNote, '{35}' AS VhlRecBuyMoneyFrom, 
                                        '{36}' AS VhlRecBuyMoneyFromOtNote, '{37}' AS VhlRecDiscardDate, '{38}' AS VhlRecDiscardReason, 
                                        '{39}' AS VhlRecDiscardReasonNote, '{40}' AS VhlRecDiscard, '{41}' AS VhlRecDiscardMoney, 
                                        '{42}' AS R_Year, '{43}' AS R_NewCarNo, '{44}' AS R_RenewYear, '{45}' AS VhlRecRemark, 
                                        '{46}' AS VhlRecCanSupportEpa, '{47}' AS VhlRecCanSupportCity, 
                                        '{48}' AS VhlRecTWD97_X, '{49}' AS VhlRecTWD97_Y, '{50}' AS VhlRecRegYear, 
                                        '{51}' AS VhlRecCatID, '{52}' AS VhlRecUseCondition, '{53}' AS WriteTime
                            ",
                                v.id,
                                v.VhlRecUptDate,
                                v.VhlRecCmpRecID,
                                v.CityName,
                                v.TownName,
                                v.VhlRecCarNo,
                                v.VhlRecModel,
                                v.VhlRecCompany,
                                v.VhlRecVhlBotCmpID,
                                v.VhlRecBotOtCountry,
                                v.VhlRecBotOtManufacturer,
                                v.VhlRecVhlBdyCmpID,
                                v.VhlRecBdyOtCountry,
                                v.VhlRecBotOtManufacturer1,
                                v.VhlRecPrdDate,
                                v.VhlRecBuyDate,
                                v.VhlRecRptCarKindID,
                                v.VhlRecCapacity,
                                v.VhlRecCapOtNote,
                                v.VhlRecGear,
                                v.VhlRecGearCountF,
                                v.VhlRecExhaust,
                                v.VhlRecFuel,
                                v.VhlRecFuelAdd,
                                v.VhlRecSeat,
                                v.VhlRecLoad,
                                v.VhlRecGrossWeight,
                                v.VhlRecAdditionItem,
                                v.VhlRecAdditionItemOtNote,
                                v.VhlRecBuyCompany,
                                v.VhlRecRealBuyDate,
                                v.VhlRecBuyPrice,
                                v.VhlRecWarrantyDate,
                                v.VhlRecBuyWayID,
                                v.VhlRecBuyWayOtNote,
                                v.VhlRecBuyMoneyFrom,
                                v.VhlRecBuyMoneyFromOtNote,
                                v.VhlRecDiscardDate,
                                v.VhlRecDiscardReason,
                                v.VhlRecDiscardReasonNote,
                                v.VhlRecDiscard,
                                v.VhlRecDiscardMoney,
                                v.R_Year,
                                v.R_NewCarNo,
                                v.R_RenewYear,
                                v.VhlRecRemark,
                                v.VhlRecCanSupportEpa,
                                v.VhlRecCanSupportCity,
                                v.VhlRecTWD97_X,
                                v.VhlRecTWD97_Y,
                                v.VhlRecRegYear,
                                v.VhlRecCatID,
                                v.VhlRecUseCondition,
                                v.WriteTime.ToShortDateString());

                            sql += @"
                            " + _dataSql;
                        }

                        //小備註(AR4,5資料更新)
                        dbMyData.Database.ExecuteSqlCommand(sql);
                    }

                    //執行SP
                    string sql_2 = @"
                            --清空車輛
                            TRUNCATE TABLE Vehicle 
                            
                            --SP：車輛(sp_ApiToVehicle)
                            Exec sp_ApiToVehicle
                        ";

                    //小備註(執行SP)
                    dbMyData.Database.ExecuteSqlCommand(sql_2);
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
