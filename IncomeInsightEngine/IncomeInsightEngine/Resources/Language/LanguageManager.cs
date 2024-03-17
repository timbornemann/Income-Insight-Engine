using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IncomeInsightEngine.Resources.Language
{
    public class LanguageManager
    {
        public enum Language
        {
            English,
            German,        
        }


       public static void SetLanguage(Language language)
        {
            switch (language)
            {
                case Language.English:
                    SetCulture("en-US");
                    break;
                case Language.German:
                    SetCulture("de-DE");
                    break;
            }
        }



      private static void SetCulture(string cultureCode)
        {
            CultureInfo culture = new CultureInfo(cultureCode);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }


    }
}
