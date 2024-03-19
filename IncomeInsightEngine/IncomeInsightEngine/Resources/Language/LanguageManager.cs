using System.Globalization;
using System.Threading;

namespace IncomeInsightEngine.Resources.Language
{
    /// <summary>
    /// Manages language settings for the application, allowing dynamic changes to the application's culture and UI culture.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to switch the application's current culture and UI culture to support internationalization.
    /// It uses an enumeration to represent supported languages and provides a method to switch cultures at runtime.
    /// </remarks>
    public class LanguageManager
    {
        public enum Language
        {
            English,
            German,        
        }

        /// <summary>
        /// Sets the application's current culture and UI culture to the specified language.
        /// </summary>
        /// <param name="language">The language to set. Must be a member of the Language enumeration.</param>
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

        /// <summary>
        /// Sets the current thread's culture and UI culture to the specified culture code.
        /// </summary>
        /// <param name="cultureCode">The culture code to set, e.g., "en-US" for English, "de-DE" for German.</param>
        /// <remarks>
        /// This method is used internally to change the culture of the current thread based on the specified culture code.
        /// </remarks>
        private static void SetCulture(string cultureCode)
        {
            CultureInfo culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
