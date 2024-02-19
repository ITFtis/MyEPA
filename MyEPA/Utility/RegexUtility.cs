namespace MyEPA
{
    public static class RegexUtility
    {
        public static string Number = "^[0-9]*$";
        /// <summary>
        /// 手機
        /// </summary>
        public static string Mobile = "^09[0-9]{2}-[0-9]{6}";
        /// <summary>
        /// 手機沒有 - 
        /// </summary>
        public static string Mobile2 = "^09[0-9]{2}[0-9]{6}";
        /// <summary>
        /// 手機 兩個 -
        /// </summary>
        public static string Mobile3 = "^09[0-9]{2}-[0-9]{3}-[0-9]{3}";
        /// <summary>
        /// 室內電話
        /// </summary>
        public static string TEL = "0\\d{1,2}-(\\d{6,8})(#\\d{1,5}){0,1}";

        public static string Email = "^([a-zA-Z0-9_\\.\\-\\+])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$";

    }
}