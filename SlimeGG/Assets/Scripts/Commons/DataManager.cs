using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string jsonData = File.ReadAllText("Assets/Resources/Jsons/Monsters/Infants/Ore" + ".json");
        MonsterSpeciesInfo test = JsonUtility.FromJson<MonsterSpeciesInfo>(jsonData);
        //print(Resources.Load<TextAsset>(
        //        "Jsons/Monsters/Infants/Ore"
        //        ).text
        //    );
        //print(test);

        MonsterSpeciesInfo wTest = new MonsterSpeciesInfo(
            "Ore", "Infant", new List<string>() { "Earth", "Golem" }, new List<string>() { "Charge", "Scratch" },
            100, 100, 100, new List<float>() { 1f, 1.5f });
        print(wTest);
        File.WriteAllText("Assets/Resources/Jsons/Monsters/Infants/Ore2" + ".json", JsonUtility.ToJson(wTest, true));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
