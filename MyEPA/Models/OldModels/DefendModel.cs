using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class DefendModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DutyId { get; set; }

        public int DiasterId { get; set; }

        public int CityId { get; set; }

        public int TownId { get; set; }

        public int Status { get; set; }

        public string UpdateReason { get; set; }

        public DateTime CreateDate { get; set; }

        public string Creator { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Updator { get; set; }

        public DateTime? ConfirmTime { get; set; }

        public string Confirmor { get; set; }

    }
    public class UnNotificationJoinDefendModel
    {
        public int CityId { get; set; }
        public string Town { get; set; }
        public string Name { get; set; }
        public string OfficePhone { get; set; }
    }
    public class DefendViewModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        public int? DiasterId { get; set; }
        public string UpdateReason { get; set; }

        public DefendStatusEnum? Status { get; set; }
        public List<DefendDutyQuestionJoinModel> Questions { get; set; }
    }
    public class DefendConfirmViewModel
    {
        public int Id { get; set; }
        public string TownName { get; set; }
        public DefendStatusEnum Status { get; set; }
        public string UpdateReason { get; set; }
        public List<DefendDutyQuestionJoinModel> Questions { get; set; }
    }

    public class DefendTeamConfirmViewModel
    {
        public int? Id { get; set; }
        [DisplayName("縣市通報細目及資料確認")]
        public DefendStatusEnum Status { get; set; }
        [DisplayName("縣市名稱")]
        public string CityName { get; set; }
        public int CityId { get; set; }
    }

    public class DefendQuestionModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int DefendId { get; set; }

        public bool Ans { get; set; }

        public string Remark { get; set; }

        public DateTime UpdateDate { get; set; }
    }

    public class DefendDutyQuestionModel
    {
        public int DutyId { get; set; }

        public int QuestionId { get; set; }

        public int Sort { get; set; }

    }
    /// <summary>
    /// DefendDutyQuestion 
    /// Join Question 
    /// Left Join DefendQuestion
    /// </summary>
    public class DefendDutyQuestionJoinModel
    { 
        public int? Id { get; set; }
        public int? DefendId { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public bool? Ans { get; set; }
        public string Remark { get; set; }
        public int Sort { get; set; }
    }
}