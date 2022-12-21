using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonsterSpeciesInfo temp = CommonFunctions.LoadObjectFromJson<MonsterSpeciesInfo>(
            "Assets/Resources/Jsons/Monsters/Infants/Ore"
            );
        LocalDictionary.monsters[MonsterSpeciesEnum.Ore] = temp;
        temp = CommonFunctions.LoadObjectFromJson<MonsterSpeciesInfo>(
            "Assets/Resources/Jsons/Monsters/Eggs/Egg"
            );
        LocalDictionary.monsters[MonsterSpeciesEnum.Egg] = temp;

        LocalStorage.monsters = CommonFunctions.LoadObjectFromJson<List<MonsterInfo>>(
            "Assets/Resources/Jsons/Save/Monsters"
            );



        //SkillStat chargeTest = CommonFunctions.LoadObjectFromJson<SkillStat>(
        //    "Assets/Resources/Jsons/Monsters/Skills/Charge"
        //    );
        //print(chargeTest);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
