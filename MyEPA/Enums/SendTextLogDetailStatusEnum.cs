using System.ComponentModel;

namespace MyEPA.Enums
{


    public enum SendTextLogDetailStatusEnum
    {
        [Description("訊息已成功傳送給電信端，電信基地台與受話⽅⼿機溝通中")]
        Wait = 0,
        [Description("成功送達⼿機")]
        Success = 100,
        [Description("電信端回覆因受話⽅⼿機關機/訊號不良/簡訊功能異常等原因，該訊息無法送達受話⽅⼿機")]
        Failure101 = 101,
        [Description("電信端回覆因網路系統/基地台設備異常等原因，該訊息無法送達受話⽅⼿機")]
        Failure102 = 102,
        [Description("電信端回覆因受話⽅⼿機⾨號為空號或停⽤中，該訊息無法送達受話⽅⼿機")]
        Failure103 = 103,
        [Description("電信端回覆因受話⽅⼿機規格不符(⼭寨機或海外機)，該訊息無法送達受話⽅⼿機")]
        Failure104 = 104,
        [Description("電信端回覆因受話⽅⼿機設備問題/⼿機出現未預期錯誤等原因，該訊息無法送達受話⽅⼿機")]
        Failure105 = 105,
        [Description("電信端回覆因系統傳送時發⽣非預期錯誤，該訊息無法送達受話⽅⼿機")]
        Failure106 = 106,
        [Description("電信端回覆因受話⽅⼿機關機/訊號不良/簡訊功能異常等原因，該訊息無法送達受話⽅⼿機")]
        Failure107 = 107,
        [Description("預約簡訊")]
        Reservation = 300,
        [Description("取消預約簡訊")]
        CancelReservation = 303,
        [Description("表該⾨號為國際⾨號，請⾄系統設定開啟國際簡訊發送功能")]
        International = 500,
        [Description("參數錯誤，該訊息傳送失敗")]
        Error1 = -1,
        [Description("API 帳號或密碼錯誤，該訊息傳送失敗")]
        Error2 = -2,
        [Description("受話⽅⼿機號碼錯誤或是簡訊⿊名單，該訊息傳送失敗")]
        Error3 = -3,
        [Description("訊息預計發送時間已逾期 24 ⼩時以上，該訊息傳送失")]
        Error4 = -4,

    }
}