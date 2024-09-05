using DocumentFormat.OpenXml.Drawing.Charts;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace MyEPA.Services
{
    public class DamageService
    {
        DamageRepository DamageRepository = new DamageRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();
        CityRepository CityRepository = new CityRepository();
        TownRepository TownRepository = new TownRepository();
        LandfillRepository LandfillRepository = new LandfillRepository();
        IncineratorRepository IncineratorRepository = new IncineratorRepository();
        FileDataService FileService = new FileDataService();

        private List<DateTime> GetActiveDays(DateTime start, DateTime end)
        {
            List<DateTime> activeDays = new List<DateTime>();
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                activeDays.Add(date);
            }
            return activeDays;
        }
        public List<DamageConfirmList> GetConfirmList(DamageConfirmListFilterParameter filter)
        {
            DateTime startTime = filter.Diaster.StartTime;
            DateTime endTime = filter.Diaster.EndTime;
            if (filter.DamageFilterParameter.ReportDay.HasValue)
            {
                startTime = filter.DamageFilterParameter.ReportDay.Value;
                endTime = filter.DamageFilterParameter.ReportDay.Value;
            }
            else if (filter.DamageFilterParameter.CleanDay.HasValue)
            {
                startTime = filter.DamageFilterParameter.CleanDay.Value;
                endTime = filter.DamageFilterParameter.CleanDay.Value;
            }
            var activeDays = GetActiveDays(startTime, endTime);

            //環保局確認清潔隊
            var damages = DamageRepository.GetListByFilter(filter.DamageFilterParameter).Where(a => a.TownId != null && a.TownId != 0);
            MultiKeyDictionary<int, DateTime?, DamageJoinModel> models;
            if (filter.DamageFilterParameter.CleanDay.HasValue)
            {
                models = damages.ToMultiKeyDictionary(e => e.TownId, e => e.CleanDay, e => e);
            }
            else
            {
                models = damages.ToMultiKeyDictionary(e => e.TownId, e => e.ReportDay, e => e);
            }

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                CityIds = filter.DamageFilterParameter.CityIds
            }).ToDictionary(e => e.Id, e => e);

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = filter.DamageFilterParameter.CityIds,
                IsTown = true,
            });
            List<DamageConfirmList> result = new List<DamageConfirmList>();
            foreach (DateTime date in activeDays)
            {
              
                foreach (TownModel town in towns)
                {
                    DamageConfirmList item = null;
                    if (models.ContainsKey(town.Id, date))
                    {
                        var model = models[town.Id, date];
                        item = model.ConvertToModel<DamageJoinModel, DamageConfirmList>((input, oupput) =>
                        {

                        });
                    }
                    else
                    {
                        item = new DamageConfirmList
                        {
                            CityId = town.CityId,
                            CityName = citys.GetValue(town.CityId)?.City,
                            TownId = town.Id,
                            TownName = town.Name,
                            IsDamage = null,
                            ReportDay = date,
                            CleanDay = date,
                        };
                    }
                    result.Add(item);
                }
            }
            return result;
        }
        public List<DamageJoinModel> GetByFilter(DamageFilterParameter filter)
        {
            var model = DamageRepository.GetListByFilter(filter);
            
            return model;
        }

        /// <summary>
        /// 環境清理資料
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<DamageJoinModel> GetCleanByFilter(DamageFilterParameter filter)
        {
            var model = DamageRepository.GetCleanListByFilter(filter);

            return model;
        }

        public DamageViewModel Get(int id)
        {
            DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter 
            {
                Ids = id.ToListCollection()
            }).FirstOrDefault();

            if(damage == null)
            {
                throw new Exception("不存在");
            }
            var diaster = DiasterRepository.Get(damage.DiasterId);
            DamageViewModel result = new DamageViewModel 
            {
                DiasterName = diaster?.DiasterName,
                DamageArea = damage.DamageArea,
                DisinfectArea = damage.DisinfectArea,
                FloodArea = damage.FloodArea,
                NationalArmyQuantity = damage.NationalArmyQuantity,
                CityId = damage.CityId,
                CityName = damage.CityName,
                CleaningMemberQuantity = damage.CleaningMemberQuantity,
                CLE_Disinfect = damage.CLE_Disinfect,
                CLE_Garbage = damage.CLE_Garbage,
                CLE_MUD = damage.CLE_MUD,
                CLE_Trash = damage.CLE_Trash,
                ConfirmTime = damage.ConfirmTime,
                CreateDate = damage.CreateDate,
                CleanCreateDate = damage.CleanCreateDate,
                DamagePlace = damage.DamagePlace,
                DiasterId = damage.DiasterId,
                CleanDay = damage.CleanDay,
                DisinfectDate = damage.DisinfectDate,
                DumpSiteDesc = damage.DumpSiteDesc,
                DutyId = damage.DutyId,
                FileId = damage.FileId,
                File = null,
                Id = damage.Id,
                ImageFile = null,
                ImageFileId = damage.ImageFileId,
                CCFile = null,
                CCImageFile = null,
                IncinerationPlantDesc = damage.IncinerationPlantDesc,
                IncineratorIds = damage.IncineratorIds,
                IsDamage = damage.IsDamage,
                IsDamageClean = damage.IsDamageClean,
                LandfillIds = damage.LandfillIds,
                Note = damage.Note,
                Other = damage.Other,
                ProcessDesc = damage.ProcessDesc,
                CLE_DisinfectorL = damage.CLE_DisinfectorL,
                CLE_DisinfectorW = damage.CLE_DisinfectorW,
                CLE_EquipmentNum = damage.CLE_EquipmentNum,
                CLE_EquipmentDesc = damage.CLE_EquipmentDesc,
                CLE_CarNum = damage.CLE_CarNum,
                CLE_CarDesc = damage.CLE_CarDesc,
                PR_Garbage = damage.PR_Garbage,
                ReportDay = damage.ReportDay,
                Status = damage.Status,
                CleanStatus = damage.CleanStatus,
                TownId = damage.TownId,
                TownName = damage.TownName,
                UpdateDate = damage.UpdateDate,
                CleanUpdateDate = damage.CleanUpdateDate,
            };

            if (damage.FileId.HasValue)
            {
                var file = FileService.Get(damage.FileId.Value);
                result.File = file;
            }

            if (damage.ImageFileId.HasValue)
            {
                var imageFile = FileService.Get(damage.ImageFileId.Value);
                result.ImageFile = imageFile;
            }
            if(string.IsNullOrWhiteSpace(result.IncineratorIds))
            {
                result.IncineratorIds = "";
            }
            if (string.IsNullOrWhiteSpace(result.LandfillIds))
            {
                result.LandfillIds = "";
            }
            result.InputIncineratorIds = result.IncineratorIds?.Split(',').Select(e => e.TryToInt()).Where(e => e.HasValue).Select(e => e.Value).ToList();
            result.InputLandfillIds = result.LandfillIds?.Split(',').Select(e => e.TryToInt()).Where(e => e.HasValue).Select(e => e.Value).ToList();
            return result;
        }

        public void Update(UserBriefModel user, DamageViewModel model, Dictionary<string, List<HttpPostedFileBase>> files)
        {
            DamageViewModel damage = Get(model.Id);

            if(damage == null)
            {
                throw new Exception("不存在");
            }

            if (model.InputIncineratorIds == null)
            {
                model.InputIncineratorIds = new List<int>();
            }
            if (model.InputLandfillIds == null)
            {
                model.InputLandfillIds = new List<int>();
            }

            DamageModel entity = damage;

            //災情通報
            UploadFile(user, model, files, "File");
            UploadFile(user, model, files, "ImageFile");

            entity.DamagePlace = model.DamagePlace;
            entity.DumpSiteDesc = model.DumpSiteDesc;
            entity.LandfillIds = string.Join(",", model.InputLandfillIds);
            entity.IncinerationPlantDesc = model.IncinerationPlantDesc;
            entity.IncineratorIds = string.Join(",", model.InputIncineratorIds);
            entity.Other = model.Other;
            //entity.ProcessDesc = model.ProcessDesc;
            entity.Note = model.Note;

            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.IsDamage = IsDamage(entity); //true;
            DamageRepository.Update(entity);
        }

        /// <summary>
        /// 環境清理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="files"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateCC(UserBriefModel user, DamageViewModel model, Dictionary<string, List<HttpPostedFileBase>> files)
        {
            DamageViewModel damage = Get(model.Id);

            if (damage == null)
            {
                throw new Exception("不存在");
            }          

            DamageModel entity = damage;

            //環境清理
            UploadFile(user, model, files, "CCFile");
            UploadFile(user, model, files, "CCImageFile");

            entity.DisinfectDate = model.DisinfectDate;
            entity.DisinfectArea = model.DisinfectArea;
            entity.CLE_MUD = model.CLE_MUD;
            entity.CLE_Garbage = model.CLE_Garbage;
            entity.CleaningMemberQuantity = model.CleaningMemberQuantity;
            entity.NationalArmyQuantity = model.NationalArmyQuantity;
            entity.CLE_DisinfectorL = model.CLE_DisinfectorL;
            entity.CLE_DisinfectorW = model.CLE_DisinfectorW;
            entity.CLE_EquipmentNum = model.CLE_EquipmentNum;
            entity.CLE_EquipmentDesc = model.CLE_EquipmentDesc;
            entity.CLE_CarNum = model.CLE_CarNum;
            entity.CLE_CarDesc = model.CLE_CarDesc;
            entity.Note = model.Note;

            entity.CleanUpdateDate = DateTimeHelper.GetCurrentTime();
            entity.IsDamageClean = true;
            DamageRepository.Update(entity);
        }

        private bool UploadFile(UserBriefModel user, DamageViewModel model, Dictionary<string, List<HttpPostedFileBase>> files, string keyName)
        {
            SourceTypeEnum sourceType;
            if (files.ContainsKey(keyName) == false)
            {
                return false;
            }
            switch (keyName)
            {
                case "ImageFile":
                    sourceType = SourceTypeEnum.DamageImage;
                    break;
                case "File":
                    sourceType = SourceTypeEnum.DamageFile;
                    break;
                case "CCImageFile":
                    sourceType = SourceTypeEnum.DamageCCImage;
                    break;
                case "CCFile":
                    sourceType = SourceTypeEnum.DamageCCFile;
                    break;
                default:
                    return false;
            }

            List<HttpPostedFileBase> fileBases = files[keyName];

            foreach (var file in fileBases)
            {
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = sourceType,
                    User = user.UserName
                }, false);
            }
            return fileBases.Any();
        }

        public List<DamageReportModel> GetReport(DamageReportFilterModel filter)
        {
            return DamageRepository.GetReport(filter);
        }

        public List<DamageTownReportModel> GetTownReport(DamageTownReportFilterModel filter)
        {
            return DamageRepository.GetTownReport(filter);
        }

        public List<DamageStatisticsCityViewModel> GetCityStatistics(DamageReportFilterModel filter)
        {
            List<DamageStatisticsModel> statistics = DamageRepository.GetStatistics(filter);

            var result = statistics.GroupBy(e => new { e.CityId, e.CityArea, e.CityName })
                .Select(e => new DamageStatisticsCityViewModel
                {
                    CityArea = e.Key.CityArea,
                    CityName = e.Key.CityName,
                    CityId = e.Key.CityId,
                    DisinfectArea = e.Sum(f => f.DisinfectArea),
                    DisinfectantLiquidAmount = e.Sum(f => f.DisinfectantLiquidAmount),
                    DisinfectantSolidAmount = e.Sum(f => f.DisinfectantSolidAmount),
                    CLE_Garbage = e.Sum(f => f.CLE_Garbage),
                    FloodArea = e.Sum(f => f.FloodArea),
                    NationalArmyQuantity = e.Sum(f => f.NationalArmyQuantity),
                    CLE_DisinfectorL = e.Sum(f => f.CLE_DisinfectorL),
                    CLE_DisinfectorW = e.Sum(f => f.CLE_DisinfectorW),
                    CLE_EquipmentNum = e.Sum(f => f.CLE_EquipmentNum),
                    CLE_CarNum = e.Sum(f => f.CLE_CarNum),
                    CleaningMemberQuantity = e.Sum(f => f.CleaningMemberQuantity),
                    CLE_MUD = e.Sum(f => f.CLE_MUD),
                    CLE_Trash = e.Sum(f => f.CLE_Trash),
                    PR_Garbage = e.Sum(f => f.PR_Garbage),
                    CleanUpdateDate = e.Max(f => f.CleanUpdateDate),
                    IsDone = e.FirstOrDefault()?.IsDone
                }).ToList();

            return result;
        }

        public List<DamageTownViewModel> GetTownList(int diasterId, DateTime? reportDay, int? cityId)
        {
            var diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<DamageTownViewModel>();
            }

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                IsCounty = true
            }).ToList();

            if (citys.IsEmptyOrNull())
            {
                return new List<DamageTownViewModel>();
            }

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                //IsTown = true,
            }).ToList();

            if (towns.IsEmptyOrNull())
            {
                return new List<DamageTownViewModel>();
            }

            List<DamageJoinModel> damages = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                TownIds = towns.Select(e => e.Id).ToList(),
                DiasterIds = diasterId.ToListCollection(),
                ReportDay = reportDay
            });

            var dates = DateTimeHelper.GetBetweenAllDates(diaster.StartTime, diaster.EndTime);

            if(reportDay.HasValue)
            {
                dates = dates.Where(e => e.Date == reportDay.Value).ToList();
            }

            if (dates.IsEmptyOrNull())
            {
                return new List<DamageTownViewModel>();
            }

            List<DamageTownViewModel> result = new List<DamageTownViewModel>();

            foreach (TownModel town in towns)
            {
                foreach (var date in dates)
                {
                    DamageJoinModel damage = damages.FirstOrDefault(e => e.TownId == town.Id 
                                                    && (e.ReportDay != null && ((DateTime)e.ReportDay).Date == date.Date));
                    if(damage == null)
                    {
                        result.Add(new DamageTownViewModel
                        {
                            CityId = town.CityId,
                            CityName = citys.Where(e => e.Id == town.CityId).Select(e => e.City).FirstOrDefault(),
                            Status = DamageStatusEnum.UnNotification,
                            ReportDay = date.Date,
                            TownId = town.Id,
                            TownName = town.Name,
                            IsTown = town.IsTown,
                        });
                    }
                    else
                    {
                        result.Add(new DamageTownViewModel 
                        {
                            Id = damage.Id,
                            CityId = damage.CityId,
                            DamageArea = damage.DamageArea,
                            FloodArea = damage.FloodArea,
                            NationalArmyQuantity = damage.NationalArmyQuantity,
                            CityName = damage.CityName,
                            CleaningMemberQuantity = damage.CleaningMemberQuantity,
                            CLE_Disinfect = damage.CLE_Disinfect,
                            CLE_MUD = damage.CLE_MUD,
                            CLE_Trash = damage.CLE_Trash,
                            DamagePlace = damage.DamagePlace,
                            DumpSiteDesc = damage.DumpSiteDesc,
                            IncinerationPlantDesc = damage.IncinerationPlantDesc,
                            Other = damage.Other,
                            PR_Garbage = damage.PR_Garbage,
                            ReportDay = damage.ReportDay,
                            Status = damage.Status,
                            TownId = damage.TownId,
                            TownName = damage.TownName,
                            ConfirmTime = damage.ConfirmTime,
                            UpdateDate = damage.UpdateDate,
                            CLE_Garbage = damage.CLE_Garbage,
                            IsDamage = damage.IsDamage,
                            IsTown = town.IsTown,
                            ProcessDesc = damage.ProcessDesc,
                            Note = damage.Note,
                        });
                    }
                }
            }

            return result.OrderBy(e => e.CityId).ThenByDescending(e => e.ReportDay).ThenBy(e => e.TownId).ToList();
        }

        public List<DamageTownViewModel> GetTownCCList(int diasterId, DateTime? cleanDay, int? cityId)
        {
            var diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<DamageTownViewModel>();
            }

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                IsCounty = true
            }).ToList();

            if (citys.IsEmptyOrNull())
            {
                return new List<DamageTownViewModel>();
            }

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                //IsTown = true,
            }).ToList();

            if (towns.IsEmptyOrNull())
            {
                return new List<DamageTownViewModel>();
            }

            List<DamageJoinModel> damages = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                TownIds = towns.Select(e => e.Id).ToList(),
                DiasterIds = diasterId.ToListCollection(),
                CleanDay = cleanDay
            });

            var dates = DateTimeHelper.GetBetweenAllDates(diaster.StartTime, diaster.EndTime);

            if (cleanDay.HasValue)
            {
                dates = dates.Where(e => e.Date == cleanDay.Value).ToList();
            }

            if (dates.IsEmptyOrNull())
            {
                return new List<DamageTownViewModel>();
            }

            List<DamageTownViewModel> result = new List<DamageTownViewModel>();

            foreach (TownModel town in towns)
            {
                foreach (var date in dates)
                {
                    DamageJoinModel damage = damages.FirstOrDefault(e => e.TownId == town.Id
                                                    && (e.CleanDay != null && ((DateTime)e.CleanDay).Date == date.Date));
                    if (damage == null)
                    {
                        result.Add(new DamageTownViewModel
                        {
                            CityId = town.CityId,
                            CityName = citys.Where(e => e.Id == town.CityId).Select(e => e.City).FirstOrDefault(),
                            CleanStatus = DamageStatusEnum.UnNotification,
                            CleanDay = date.Date,
                            TownId = town.Id,
                            TownName = town.Name,
                            IsTown = town.IsTown,
                        });
                    }
                    else
                    {
                        result.Add(new DamageTownViewModel
                        {
                            Id = damage.Id,
                            CityId = damage.CityId,
                            DamageArea = damage.DamageArea,
                            FloodArea = damage.FloodArea,
                            NationalArmyQuantity = damage.NationalArmyQuantity,
                            DisinfectArea = damage.DisinfectArea,
                            CLE_DisinfectorL = damage.CLE_DisinfectorL,
                            CLE_DisinfectorW = damage.CLE_DisinfectorW,
                            CLE_EquipmentDesc = damage.CLE_EquipmentDesc,
                            CLE_CarDesc = damage.CLE_CarDesc,
                            CityName = damage.CityName,
                            CleaningMemberQuantity = damage.CleaningMemberQuantity,
                            CLE_Disinfect = damage.CLE_Disinfect,
                            CLE_MUD = damage.CLE_MUD,
                            CLE_Trash = damage.CLE_Trash,
                            DamagePlace = damage.DamagePlace,
                            DumpSiteDesc = damage.DumpSiteDesc,
                            IncinerationPlantDesc = damage.IncinerationPlantDesc,
                            Other = damage.Other,
                            PR_Garbage = damage.PR_Garbage,
                            CleanDay = damage.CleanDay,
                            CleanStatus = damage.CleanStatus,
                            TownId = damage.TownId,
                            TownName = damage.TownName,
                            UpdateDate = damage.UpdateDate,
                            CleanUpdateDate = damage.CleanUpdateDate,
                            CleanConfirmTime = damage.CleanConfirmTime,
                            CLE_Garbage = damage.CLE_Garbage,
                            DisinfectDate = damage.DisinfectDate,
                            IsDamageClean = damage.IsDamageClean,
                            IsTown = town.IsTown,
                        });
                    }
                }
            }

            return result.OrderBy(e => e.CityId).ThenByDescending(e => e.CleanDay).ThenBy(e => e.TownId).ToList();
        }

        public DamageModel UpdateMemo(DamageMemoViewModel model)
        {
            DamageModel damage = GetDamage(model.DamageId);
            damage.Note = model.Note;
            DamageRepository.Update(damage);
            return damage;
        }

        public DamageMemoViewModel GetDamageMemo(int id, FacilityDamageTypeEnum type)
        {
            DamageModel damage = GetDamage(id);
            return new DamageMemoViewModel 
            {
                DamageId = damage.Id,
                Note = damage.Note,
                Type = type
            };
        }

        public DamageModel GetDamage(int damageId)
        {
            DamageModel damage = DamageRepository.Get(damageId);
            if (damage == null)
            {
                throw new Exception("資料不存在");
            }
            return damage;
        }
        public DamageModel UpdateCorpsHandlingSituation(CorpsHandlingSituationViewModel model)
        {
            DamageModel damage = GetDamage(model.DamageId);
            damage.ProcessDesc = model.CorpsHandlingSituation;
            DamageRepository.Update(damage);
            return damage;
        }

        public CorpsHandlingSituationViewModel GetCorpsHandlingSituation(int damageId, FacilityDamageTypeEnum type)
        {
            var damage = DamageRepository.Get(damageId);
            return new CorpsHandlingSituationViewModel 
            {
                CorpsHandlingSituation = damage.ProcessDesc,
                DamageId = damage.Id,
                Type = type
            };
        }

        
        public DamageJoinModel GetDamageByDate(int cityId, int diasterId, int? townId, DateTime? date)
        {
            //先判斷是否有災情回報
            DamageFilterParameter filter = new DamageFilterParameter
            {
                CityIds = cityId.ToListCollection(),
                DiasterIds = diasterId.ToListCollection(),
                ReportDay = date,
                TownId = townId,
            };

            DamageJoinModel damage = DamageRepository.GetListByFilter(filter).FirstOrDefault();

            if (damage == null)
            {
                //否則取環境清理
                filter = new DamageFilterParameter
                {
                    CityIds = cityId.ToListCollection(),
                    DiasterIds = diasterId.ToListCollection(),
                    CleanDay = date,
                    TownId = townId,
                };

                damage = DamageRepository.GetListByFilter(filter).FirstOrDefault();
            }

            return damage;
        }

        /// <summary>
        /// 取得DamageModel 利用通報日期
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="hType">1.災情通報2.環境清理</param>
        public void NotDamage(UserBriefModel user, DamageModel model, int hType)
        {
            int cityId = user.CityId;

            DamageJoinModel join = null;
            if (hType == 1)
            {
                join = GetDamageByDate(cityId, model.DiasterId, model.TownId, model.ReportDay);
            }
            else if (hType == 2)
            {
                join = GetDamageByDate(cityId, model.DiasterId, model.TownId, model.CleanDay);
            }

            DamageModel damage = null;
            //本日無災情
            if (join != null)
            {
                damage = join;
            }
            else
            {
                damage = new DamageModel
                {
                    CityId = cityId,
                    TownId = model.TownId,
                    DiasterId = model.DiasterId,                   
                };
            }

            if (hType == 1)
            {
                damage.CreateDate = damage.CreateDate != null ? damage.CreateDate : DateTimeHelper.GetCurrentTime();
                damage.UpdateDate = DateTimeHelper.GetCurrentTime();
                damage.Status = DamageStatusEnum.Waiting;
                damage.IsDamage = IsDamage(damage); //false;
                damage.ReportDay = model.ReportDay;
            }
            else if (hType == 2)
            {
                damage.CleanCreateDate = damage.CleanCreateDate != null ? damage.CleanCreateDate : DateTimeHelper.GetCurrentTime();
                damage.CleanUpdateDate = DateTimeHelper.GetCurrentTime();
                damage.CleanStatus = DamageStatusEnum.Waiting;
                damage.IsDamageClean = false;
                damage.CleanDay = model.CleanDay;
            }

            //儲存
            if (join == null)
            {
                DamageRepository.Create(damage);
            }
            else
            {
                DamageRepository.Update(damage);
            }
        }

        public void Confirm(UserBriefModel user, int id, DamageStatusEnum status, int hType)
        {
            DamageModel result = DamageRepository.Get(id);

            //無資料或是確認過不回應
            if (result == null)
            {
                return;
            }

            if (hType == 1)
            {
                result.Status = status;

                if (user.Duty == DutyEnum.Team)
                {
                    result.TeamConfirmTime = DateTimeHelper.GetCurrentTime();
                }
                else
                {
                    result.ConfirmTime = DateTimeHelper.GetCurrentTime();
                }
            }
            else if (hType == 2)
            {
                result.CleanStatus = status;

                if (user.Duty == DutyEnum.Team)
                {
                    result.CleanTeamConfirmTime = DateTimeHelper.GetCurrentTime();
                }
                else
                {
                    result.CleanConfirmTime = DateTimeHelper.GetCurrentTime();
                }
            }           

            DamageRepository.Update(result);
        }
        public List<DamageTeamConfirmViewModel> GetConfirmList(int diasterId, AreaEnum area, int? cityId)
        {
            var diaster = DiasterRepository.Get(diasterId);

            if(diaster == null)
            {
                return new List<DamageTeamConfirmViewModel>();
            }

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                AreaIds = area.ToInteger().ToListCollection(),
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                IsCounty = true
            }).ToList();

            if(citys.IsEmptyOrNull())
            {
                return new List<DamageTeamConfirmViewModel>();
            }

            //全部鄉鎮
            var allTowns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                //IsTown = false
            }).ToList();

            if (allTowns.IsEmptyOrNull())
            {
                return new List<DamageTeamConfirmViewModel>();
            }

            //縣市(含鄉鎮)通報清單
            List<DamageJoinModel> allDamages =
               DamageRepository.GetListByFilter(new DamageFilterParameter
               {
                   TownIds = allTowns.Select(e => e.Id).ToList(),
                   DiasterIds = diasterId.ToListCollection(),
                   CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>()
               });

            //縣市通報清單
            List<DamageJoinModel> damages = allDamages.Where(a => a.IsTown == false).ToList();              

            List<DamageTeamConfirmViewModel> result = new List<DamageTeamConfirmViewModel>();

            var dates = DateTimeHelper.GetBetweenAllDates(diaster.StartTime, diaster.EndTime);

            //縣市環保局鄉鎮
            List<TownModel> cityTowns = allTowns.Where(a => a.IsTown == false).ToList();
            foreach (TownModel town in cityTowns)
            {
                foreach (var date in dates)
                {
                    //指定縣市、鄉鎮災害數量合計 (篩選日期)
                    var sss = allDamages.Where(a => a.CityId == town.CityId)
                                .Where(e => (e.ReportDay != null && ((DateTime)e.ReportDay).Date == date.Date));
                    
                    int cityDamageNum = sss.Where(a => a.IsTown == false).Where(a => a.IsDamage != null && (a.IsDamage ?? false) == true).Count();
                    int townDamageNum = sss.Where(a => a.IsTown == true).Where(a => a.IsDamage != null && (a.IsDamage ?? false) == true).Count();

                    DamageJoinModel damage = damages.FirstOrDefault(e => e.TownId == town.Id
                                                            && (e.ReportDay != null && ((DateTime)e.ReportDay).Date == date.Date));

                    result.Add(new DamageTeamConfirmViewModel
                    {
                        CityId = town.CityId,
                        CityName = citys.Where(e => e.Id == town.CityId).Select(e => e.City).FirstOrDefault(),
                        TownId = town.Id,
                        Status = damage == null ? DamageStatusEnum.UnNotification : damage.Status,
                        Id = damage?.Id,
                        ReportDay = date.Date,
                        TeamConfirmTime = damage == null ? null : damage.TeamConfirmTime,
                        CityDamageNum = cityDamageNum,
                        TownDamageNum = townDamageNum,
                    });
                }
            }

            return result;
        }

        public List<DamageTeamConfirmViewModel> GetConfirmCCList(int diasterId, AreaEnum area, int? cityId)
        {
            var diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<DamageTeamConfirmViewModel>();
            }

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                AreaIds = area.ToInteger().ToListCollection(),
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                IsCounty = true
            }).ToList();

            if (citys.IsEmptyOrNull())
            {
                return new List<DamageTeamConfirmViewModel>();
            }

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                IsTown = false
            }).ToList();

            if (towns.IsEmptyOrNull())
            {
                return new List<DamageTeamConfirmViewModel>();
            }

            List<DamageJoinModel> damages =
               DamageRepository.GetListByFilter(new DamageFilterParameter
               {
                   TownIds = towns.Select(e => e.Id).ToList(),
                   DiasterIds = diasterId.ToListCollection(),
                   CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>()
               });

            List<DamageTeamConfirmViewModel> result = new List<DamageTeamConfirmViewModel>();

            var dates = DateTimeHelper.GetBetweenAllDates(diaster.StartTime, diaster.EndTime);

            foreach (TownModel town in towns)
            {
                foreach (var date in dates)
                {
                    DamageJoinModel damage = damages.FirstOrDefault(e => e.TownId == town.Id
                                                            && (e.CleanDay != null && ((DateTime)e.CleanDay).Date == date.Date));

                    result.Add(new DamageTeamConfirmViewModel
                    {
                        CityId = town.CityId,
                        CityName = citys.Where(e => e.Id == town.CityId).Select(e => e.City).FirstOrDefault(),
                        TownId = town.Id,
                        CleanStatus = damage == null ? DamageStatusEnum.UnNotification : damage.CleanStatus,
                        Id = damage?.Id,
                        CleanDay = date.Date,
                        CleanTeamConfirmTime = damage == null ? null : damage.CleanTeamConfirmTime,
                    });
                }
            }

            return result;
        }

        public List<DamageStatisticsTownViewModel> GetTownStatistics(DamageReportFilterModel filter)
        {
            List<DamageStatisticsModel> result = DamageRepository.GetStatistics(filter);
            return result.Select(e => e.ConvertToModel<DamageStatisticsModel, DamageStatisticsTownViewModel>()).ToList();
        }

        public List<FacilityDamageViewModel> GetFacilityDamages(DamageFilterParameter filter, FacilityDamageTypeEnum type)
        {
            var damages = DamageRepository.GetReportListByFilter(filter);
            switch (type)
            {
                  case FacilityDamageTypeEnum.ALL:
                    {
                        Dictionary<string, string> dic = LandfillRepository.GetList().ToDictionary(e => e.Id.ToString(), e => e.ContactUnit);
                        return damages.Select(e => new FacilityDamageViewModel 
                        {
                            Id = e.Id,
                            ProcessDesc = e.ProcessDesc,
                            ReportDay = e.ReportDay,
                            TownId = e.TownId,
                            TownName = e.TownName,
                            CityName = e.CityName,
                            CitySort = e.CitySort,
                            CreateDate = e.CreateDate,
                            DamagePlace = e.DamagePlace,
                            DumpSiteDesc = e.DumpSiteDesc,
                            IncinerationPlantDesc = e.IncinerationPlantDesc,
                            Other = e.Other,
                            DamageDesc = string.IsNullOrEmpty(e.DumpSiteDesc) ? string.IsNullOrEmpty(e.IncinerationPlantDesc) ? e.Other : e.IncinerationPlantDesc : e.DumpSiteDesc,
                            Note = e.Note,
                            Places = string.IsNullOrEmpty(e.LandfillIds) ? string.IsNullOrEmpty(e.LandfillIds) ? new List<string>() : e.LandfillIds.Split(',').Select(f => dic.GetValue(f)).ToList() : e.LandfillIds.Split(',').Select(f=> dic.GetValue(f)).ToList()
                        }).ToList();
                    }
                case FacilityDamageTypeEnum.DumpSiteDesc:
                    {
                        Dictionary<string, string> dic = LandfillRepository.GetList().ToDictionary(e => e.Id.ToString(), e => e.ContactUnit);
                        return damages.Select(e => new FacilityDamageViewModel 
                        {
                            Id = e.Id,
                            ProcessDesc = e.ProcessDesc,
                            ReportDay = e.ReportDay,
                            TownId = e.TownId,
                            TownName = e.TownName,
                            CityName = e.CityName,
                            CreateDate = e.CreateDate,
                            DamagePlace = e.DamagePlace,
                            DumpSiteDesc = e.DumpSiteDesc,
                            IncinerationPlantDesc = e.IncinerationPlantDesc,
                            Other = e.Other,
                            DamageDesc = e.DumpSiteDesc,
                            Note = e.Note,
                            Places = string.IsNullOrEmpty(e.LandfillIds) ? new List<string>(): e.LandfillIds.Split(',').Select(f=> dic.GetValue(f)).ToList()
                        }).ToList();
                    }
                case FacilityDamageTypeEnum.IncinerationPlantDesc:
                    {
                        Dictionary<string, string> dic = IncineratorRepository.GetList().ToDictionary(e => e.Id.ToString(), e => e.ContactUnit);
                        return damages.Select(e => new FacilityDamageViewModel
                        {
                            Id = e.Id,
                            ProcessDesc = e.ProcessDesc,
                            ReportDay = e.ReportDay,
                            TownId = e.TownId,
                            TownName = e.TownName,
                            CityName = e.CityName,
                            CreateDate = e.CreateDate,
                            DamagePlace = e.DamagePlace,
                            DumpSiteDesc = e.DumpSiteDesc,
                            IncinerationPlantDesc = e.IncinerationPlantDesc,
                            Other = e.Other,
                            DamageDesc = e.IncinerationPlantDesc,
                            Note = e.Note,
                            Places = string.IsNullOrEmpty(e.IncineratorIds) ? new List<string>() : e.IncineratorIds.Split(',').Select(f => dic.GetValue(f)).ToList()
                        }).ToList();
                    }
                case FacilityDamageTypeEnum.Other:
                    return damages.Select(e => new FacilityDamageViewModel
                    {
                        Id = e.Id,
                        ProcessDesc = e.ProcessDesc,
                        ReportDay = e.ReportDay,
                        TownId = e.TownId,
                        TownName = e.TownName,
                        CityName = e.CityName,
                        CreateDate = e.CreateDate,
                        DamagePlace = e.DamagePlace,
                        DumpSiteDesc = e.DumpSiteDesc,
                        IncinerationPlantDesc = e.IncinerationPlantDesc,
                        Other = e.Other,
                        DamageDesc = e.Other,
                        Note = e.Note,
                        Places = new List<string>()
                    }).ToList();
                default:
                    return new List<FacilityDamageViewModel>();
            }            
        }

        /// <summary>
        /// 災情通報是否為災情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsDamage(DamageModel model)
        {
            bool result = false;

            if (model == null)
                return false;

            result = !string.IsNullOrEmpty(model.DamagePlace)
                    || !string.IsNullOrEmpty(model.DumpSiteDesc)
                    || !string.IsNullOrEmpty(model.IncinerationPlantDesc)
                    || !string.IsNullOrEmpty(model.Other);

            return result;
        }
    }
}