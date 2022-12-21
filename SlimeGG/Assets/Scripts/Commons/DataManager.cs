using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonsterSpeciesInfo temp = CommonFunctions.loadObjectFromJson<MonsterSpeciesInfo>(
            "Assets/Resources/Jsons/Monsters/Infants/Ore"
            );
        LocalDictionary.monsters[MonsterSpeciesEnum.Ore] = temp;
        temp = CommonFunctions.loadObjectFromJson<MonsterSpeciesInfo>(
            "Assets/Resources/Jsons/Monsters/Eggs/Egg"
            );
        LocalDictionary.monsters[MonsterSpeciesEnum.Egg] = temp;

        LocalStorage.monsters = CommonFunctions.loadObjectFromJson<List<MonsterInfo>>(
            "Assets/Resources/Jsons/Save/Monsters"
            );
        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/TileSets/Shapes"
            ))
        {
            LocalDictionary.tileSetCoors[Enum.Parse<TileSetShapeEnum>(fileName)] =
                CommonFunctions.loadObjectFromJson<TileSetShapeStat>(
                    "Assets/Resources/Jsons/TileSets/Shapes/" + fileName
                    );
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
