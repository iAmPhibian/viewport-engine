using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewportGame.Util;

public static class EnumUtil {
    public static IEnumerable<T> GetValues<T>() {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static int CountValues<T>()
    {
        return Enum.GetValues(typeof(T)).Length;
    }

    public static T Next<T>(this T value) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), (value.ToInt() + 1) % CountValues<T>());
    }
    
    public static int ToInt(this Enum a)
    {
        return Convert.ToInt32(a);
    }
}