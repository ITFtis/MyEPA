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
            var activeDays = GetActiveDays(startTime, endTime);

            var models = DamageRepository.GetListByFilter(filter.DamageFilterParameter)
                .ToMultiKeyDictionary(e => e.TownId, e => e.ReportDay, e => e);

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                CityIds = filter.DamageFilterParameter.CityIds
            }).ToDictionary(e => e.Id, e => e);

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = filter.DamageFilterParameter.CityIds
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
                            ReportDay = date
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
                DamagePlace = damage.DamagePlace,
                DiasterId = damage.DiasterId,
                DisinfectDate = damage.DisinfectDate,
                DumpSiteDesc = damage.DumpSiteDesc,
                DutyId = damage.DutyId,
                FileId = damage.FileId,
                File = null,
                Id = damage.Id,
                ImageFile = null,
                ImageFileId = damage.ImageFileId,
                IncinerationPlantDesc = damage.IncinerationPlantDesc,
                IncineratorIds = damage.IncineratorIds,
                IsDamage = damage.IsDamage,
                LandfillIds = damage.LandfillIds,
                Note = damage.Note,
                Other = damage.Other,
                ProcessDesc = damage.ProcessDesc,
                PR_Garbage = damage.PR_Garbage,
                ReportDay = damage.ReportDay,
                Status = damage.Status,
                TownId = damage.TownId,
                TownName = damage.TownName,
                UpdateDate = damage.UpdateDate
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

            UploadFile(user, model, files, "File");
            UploadFile(user, model, files, "ImageFile");

            entity.ReportDay = model.ReportDay;
            entity.CleaningMemberQuantity = model.CleaningMemberQuantity;
            entity.CLE_Disinfect = model.CLE_Disinfect;
            entity.CLE_Garbage = model.CLE_Garbage;
            entity.CLE_MUD = model.CLE_MUD;
            entity.CLE_Trash = model.CLE_Trash;
            entity.DamageArea = model.DamageArea;
            entity.DamagePlace = model.DamagePlace;
            entity.DisinfectArea = model.DisinfectArea;
            entity.DisinfectDate = model.DisinfectDate;
            entity.DumpSiteDesc = model.DumpSiteDesc;
            entity.FloodArea = model.FloodArea;
            entity.IncinerationPlantDesc = model.IncinerationPlantDesc;
            entity.IncineratorIds = string.Join(",", model.InputIncineratorIds);
            entity.LandfillIds = string.Join(",", model.InputLandfillIds); 
            entity.NationalArmyQuantity = model.NationalArmyQuantity;
            entity.Note = model.Note;
            entity.Other = model.Other;
            entity.ProcessDesc = model.ProcessDesc;
            entity.PR_Garbage = model.PR_Garbage;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.IsDamage = true;
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
                    CleaningMemberQuantity = e.Sum(f => f.CleaningMemberQuantity),
                    CLE_MUD = e.Sum(f => f.CLE_MUD),
                    CLE_Trash = e.Sum(f => f.CLE_Trash),
                    PR_Garbage = e.Sum(f => f.PR_Garbage),
                    UpdateDate = e.Max(f => f.UpdateDate),
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
                    DamageJoinModel damage = damages.FirstOrDefault(e => e.TownId == town.Id && e.ReportDay.Date == date.Date);
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
                        });
                    }
                    else
                    {
                        result.Add(new DamageTownViewModel 
                        {
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
                            PR_Garbage = damage.PR_Garbage,
                            ReportDay = damage.ReportDay,
                            Status = damage.Status,
                            TownId = damage.TownId,
                            TownName = damage.TownName,
                            UpdateDate = damage.UpdateDate,
                            CLE_Garbage = damage.CLE_Garbage,
                            IsDamage = damage.IsDamage
                        });
                    }
                }
            }

            return result.OrderBy(e => e.CityId).ThenByDescending(e => e.ReportDay).ThenBy(e => e.TownId).ToList();
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

        public void NotDamage(UserBriefModel user, DamageModel model)
        {
            int cityId = user.CityId;
            int townId = model.TownId;
            var filter = new DamageFilterParameter
            {
                CityIds = cityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = model.ReportDay,
                TownIds = townId.ToListCollection(),
            };
            // DamageJoinModel damageJoin = DamageRepository.GetListByFilter(filter).FirstOrDefault();
            var  damageLIST = DamageRepository.GetListByFilter(filter);
            if (damageLIST.Count == 0)
            {
                DamageModel damage = new DamageModel
                {
                    CityId = cityId,
                    TownId = townId,
                    ReportDay = model.ReportDay,
                    CreateDate = DateTimeHelper.GetCurrentTime(),
                    UpdateDate = DateTimeHelper.GetCurrentTime(),
                    DiasterId = model.DiasterId,
                    Status = DamageStatusEnum.Waiting,
                    IsDamage = false,
                    DisinfectDate = DateTimeHelper.GetCurrentTime(),
                };
                DamageRepository.Create(damage);
            }
            else
            {
                foreach (DamageJoinModel damageJoin in damageLIST) 
                {

                    DamageJoinModel damage = new DamageJoinModel
                    {
                        Id = damageJoin.Id,
                        CityId = damageJoin.CityId,
                        TownId = damageJoin.TownId,
                        ReportDay = damageJoin.ReportDay,
                        CreateDate = DateTimeHelper.GetCurrentTime(),
                        UpdateDate = DateTimeHelper.GetCurrentTime(),
                        DiasterId = damageJoin.DiasterId,
                        Status = DamageStatusEnum.Waiting,
                        IsDamage = false,
                        DisinfectDate = DateTimeHelper.GetCurrentTime(),
                    };







                //    damageJoin.IsDamage = false;
                //damageJoin.UpdateDate = DateTimeHelper.GetCurrentTime();
                DamageRepository.Update(damage);
                }
            }
        }

        public void Confirm(UserBriefModel user, int id, DamageStatusEnum status)
        {
            DamageModel result = DamageRepository.Get(id);

            //無資料或是確認過不回應
            if (result == null)
            {
                return;
            }

            result.Status = status;

            if (user.Duty == DutyEnum.Team)
            {
                result.TeamConfirmTime = DateTimeHelper.GetCurrentTime();
            }
            else
            {
                result.ConfirmTime = DateTimeHelper.GetCurrentTime();
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
                    DamageJoinModel damage = damages.FirstOrDefault(e => e.TownId == town.Id && e.ReportDay.Date == date.Date);
                    result.Add(new DamageTeamConfirmViewModel
                    {
                        CityId = town.CityId,
                        CityName = citys.Where(e => e.Id == town.CityId).Select(e => e.City).FirstOrDefault(),
                        Status = damage == null ? DamageStatusEnum.UnNotification : damage.Status,
                        Id = damage?.Id,
                        ReportDay = date.Date
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
            var damages = DamageRepository.GetListByFilter(filter);
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
                            TownName = e.TownName,
                            CityName = e.CityName,
                            CreateDate = e.CreateDate,
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
                            TownName = e.TownName,
                            CityName = e.CityName,
                            CreateDate = e.CreateDate,
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
                            TownName = e.TownName,
                            CityName = e.CityName,
                            CreateDate = e.CreateDate,
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
                        TownName = e.TownName,
                        CityName = e.CityName,
                        CreateDate = e.CreateDate,
                        DamageDesc = e.Other,
                        Note = e.Note,
                        Places = new List<string>()
                    }).ToList();
                default:
                    return new List<FacilityDamageViewModel>();
            }            
        }
    }
}