using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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

    public static T loadObjectFromJson<T>(string jsonPath)
    {
        string jsonData = File.ReadAllText(jsonPath + ".json");
        return JsonConvert.DeserializeObject<T>(jsonData);
    }
    public static List<string> loadFileNamesFromFolder(string folderPath)
    {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        List<string> files = new List<string>();
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.Split(".").Length <= 2)
                files.Add(file.Name.Split(".")[0]);
        }
        return files;
    }

    public static void saveObjectToJson<T>(string jsonPath, T objectToSave)
    {
        File.WriteAllText(
            jsonPath + ".json",
            JsonConvert.SerializeObject(objectToSave, Formatting.Indented)
            );
    }
}
