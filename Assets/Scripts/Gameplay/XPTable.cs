using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG
{
	/// <summary>
	/// XPTable
	/// </summary>
	public static class XPTable
	{
        static int[] XPLevels = new int[] { 1000, 3000, 5000, 10000, 15000 };

        // Note: level is indexed from 1
        public static int GetMinXPForLevel(int level)
        {
            if (level <= 1)
                return 0;

            return XPLevels[level - 2];
        }
        // Note: level is indexed from 1
        public static int GetMaxXPForLevel(int level)
        {
            int idx = level - 1;

            if (XPLevels.Length <= idx)
                return XPLevels[XPLevels.Length - 1] * 2;

            return XPLevels[level - 1];
        }
    }
}