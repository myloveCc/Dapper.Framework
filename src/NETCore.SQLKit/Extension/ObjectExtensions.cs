using System.Collections.Generic;

namespace NETCore.SQLKit
{
    public static class ObjectExtensions
    {
        public static bool Like(this object obj, string value)
        {
            return true;
        }

        /// <summary>
        /// like '% _ _ _'
        /// </summary>
        public static bool LikeLeft(this object obj, string value)
        {
            return true;
        }

        /// <summary>
        /// like '_ _ _ %'
        /// </summary>
        public static bool LikeRight(this object obj, string value)
        {
            return true;
        }


        public static bool In<T>(this object obj, T ary)
        {
            return true;
        }
    }
}
