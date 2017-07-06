using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.utils
{
    public static class utils
    {
        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static float GetValuePercent(float value, float maxValue)
        {
            return (value * 100f) / maxValue;
        }

        //public static float XPercent(float percent)
        //{
        //    return (b.ScreenWidth / 100f) * percent;
        //}

        //public static float YPercent(float percent)
        //{
        //    return (b.ScreenHeight / 100f) * percent;
        //}

        private static int _fontSize;

        public static void Setup()
        {
            if (Screen.width >= 2560)
            {
                //_fontSize = 
            }
        }

        public static int GetFontSize()
        {
            return _fontSize;
        }
    }
}
