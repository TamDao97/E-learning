using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NTS.Common.Helpers
{
   public static class SlugHelper
    {
        public static string ConverNameToSlug (this string text)
        {
            try
            {
                text = text.ToLowerInvariant().Trim() ;
                for (int i = 32; i < 48; i++)
                {

                    text = text.Replace(((char)i).ToString(), " ");

                }
                Regex trimmer = new Regex(@"\s\s+");
                text = trimmer.Replace(text, " ");

                text = text.Replace(" ", "-");
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

                return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            catch
            {
                throw new Exception("Có lỗi phát sinh");
            }
        }
    }
}
