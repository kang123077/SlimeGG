using System;
using System.Collections.Generic;

public static class CommonFunctions
{
    public static T convertEnumFromString<T>(string target)
    {
        return (T)Enum.Parse(typeof(T), target);
    }

    public static List<T> convertEnumFromStringArr<T>(List<string> target)
    {
        List<T> res = new List<T>();
        target.ForEach((str) =>
        {
            res.Add(convertEnumFromString<T>(str));
        });
        return res;
    }
}
