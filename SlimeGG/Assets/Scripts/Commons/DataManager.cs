using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonsterSpeciesInfo oreTest = CommonFunctions.LoadObjectFromJson<MonsterSpeciesInfo>(
            "Assets/Resources/Jsons/Monsters/Infants/Ore"
            );
        print(oreTest);

        SkillStat chargeTest = CommonFunctions.LoadObjectFromJson<SkillStat>(
            "Assets/Resources/Jsons/Monsters/Skills/Charge"
            );
        print(chargeTest);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
