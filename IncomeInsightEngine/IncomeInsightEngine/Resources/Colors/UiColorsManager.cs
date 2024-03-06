using IncomeInsightEngine.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace IncomeInsightEngine.Resources.Colors
{

    public static class UiColorsManager
    {
        public static bool IsLightMode { get; set; } = true;

        public enum ColorName
        {
            LaurelGreen,
            MediumCarmine,
            Black,
            White,
            LightGray,
            DarkGrey
        }

        public static string GetColorHexCode(ColorName colorName)
        {
            switch (colorName)
            {
                case ColorName.LaurelGreen:
                    return (IsLightMode) ? "#A3B18A" : "#303D18";

                case ColorName.MediumCarmine:
                    return (IsLightMode) ? "#AA4642" : "#8F1817";

                case ColorName.Black:
                    return (IsLightMode) ? "#FF000000" : "#FFFFFFFF";

                case ColorName.White:
                    return (IsLightMode) ? "#FFFFFFFF" : "#FF000000";

                case ColorName.LightGray:
                    return (IsLightMode) ? "#FF3C3C3C" : "#FF3C3C3C";

                case ColorName.DarkGrey:
                    return (IsLightMode) ? "#FF212121" : "#FF212121";

                default:
                    return "#FF000000";

            }
        }

        public static Brush GetBrush(ColorName colorName)
        {
            return (Brush)new BrushConverter().ConvertFrom(GetColorHexCode(colorName));
        }

    }
}
