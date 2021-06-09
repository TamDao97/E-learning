using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace NTS.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Convert string yyyy-MM-dd to dd/MM/yyyy
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>dd/MM/yyyy</returns>
        public static string ConvertDateYMDToDMY(this string stringValue)
        {
            try
            {
                DateTime datetTime = DateTime.ParseExact(stringValue, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                return datetTime.ToString("dd/MM/yyyy");
            }
            catch
            {
                throw new Exception("Không đúng format yyyy-MM-dd");
            }
        }

        /// <summary>
        /// Convert date to string viet nam
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>string viet nam</returns>
        public static string ConvertDateVietNam(this DateTime? date)
        {
            if (date.HasValue)
            {
                return string.Format("Ngày {0} tháng {1} năm {2}", date.Value.Day.ToString("00"), date.Value.Month.ToString("00"), date.Value.Year);
            }
            else
            {
                return string.Format("Ngày  tháng  năm {0}", DateTime.Now.Year);
            }

        }
        public static string ConvertDateVietNam(this DateTime date)
        {
            return string.Format("Ngày {0} tháng {1} năm {2}", date.Day.ToString("00"), date.Month.ToString("00"), date.Year);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static int DateYMDDiffNowToDay(this string stringValue)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(stringValue, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                if (DateTime.Now.Date <= dateTime.Date)
                {
                    return (dateTime - DateTime.Now).Days;
                }

                return 0;

            }
            catch
            {
                throw new Exception("Không đúng format yyyy-MM-dd");
            }
        }

        public static string RemoveVietnameseString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;
        }

        public static string NTSTrim(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Trim();
        }

        private static readonly string[] VietnameseSigns = new string[]{

            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };
    }
}