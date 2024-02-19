using System;

namespace MyEPA.Helper
{
    public static class RandomHelper
    {

        /// <summary>
        /// 种子码
        /// </summary>
        private static char[] character = { '2','3','4','5','6','7','8','9',
                                            'A','B','C','D','E','F','G','H','J','K','K','M','N','P','Q','R','S','T','U','V','W','X','Y','Z'
                                          };

        /// <summary>
        /// 种子码
        /// </summary>
        private static char[] character1 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };



        /// <summary>
        /// 取得随机数(含英文)
        /// </summary>
        public static string GetRand(int count)
        {
            string strNo = "";

            //设置种子,不重复
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < count; i++)
            {
                strNo += character[rand.Next(character.Length - 1)].ToString();
            }

            return strNo;
        }


        /// <summary>
        /// 取得隨機數字
        /// </summary>
        public static int GetRandIntegerNumbers(int min, int max)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int rndNumber = rand.Next(min, max);
            return rndNumber;
        }


        /// <summary>
        /// 取得隨機數字
        /// </summary>
        /// <returns></returns>
        public static string GetRandNumbers(int length = 5)
        {
            //设置种子
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            int maxNumber = (int)(Math.Pow(10, length) - 1);

            string strNo = rand.Next(1, maxNumber).ToString().PadLeft(length, '0');

            return strNo;
        }

        /// <summary>
        /// 取得隨機數字
        /// </summary>
        /// <returns></returns>
        public static int GetRandIntegerNumbers(int length = 5)
        {
            //设置种子
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int maxNumber = (int)(Math.Pow(10, length) - 1);
            var result = rand.Next(1, maxNumber);
            return result;
        }


        /// <summary>
        /// 生成订单编号
        /// </summary>
        /// <param name="pre">前缀</param>
        public static string GetOrderNo(string pre)
        {

            string strNo = GetRand(5);

            strNo = DateTime.Now.ToString("yyMMddHHmm") + strNo;
            if (pre != string.Empty)
                strNo = pre + strNo;

            return strNo;
        }
    }
}