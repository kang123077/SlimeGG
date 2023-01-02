using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (string folderName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Monsters"
            ))
        {
            if (folderName != "Skills")
            {
                foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
                $"Assets/Resources/Jsons/Monsters/{folderName}"
                ))
                {
                    LocalDictionary.speices[fileName] =
                        CommonFunctions.loadObjectFromJson<MonsterSpeciesVO>(
                            $"Assets/Resources/Jsons/Monsters/{folderName}/{fileName}"
                            );
                }
            }
        }

        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/TileSets/Shapes"
            ))
        {
            LocalDictionary.tileSetCoors[Enum.Parse<TileSetShapeEnum>(fileName)] =
                CommonFunctions.loadObjectFromJson<TileSetShapeStat>(
                    "Assets/Resources/Jsons/TileSets/Shapes/" + fileName
                    );
        }

        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Monsters/Skills"
            ))
        {
            LocalDictionary.skills[fileName] =
                CommonFunctions.loadObjectFromJson<SkillStat>(
                    $"Assets/Resources/Jsons/Monsters/Skills/{fileName}"
                    );
        }

        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Fields"
            ))
        {
            LocalDictionary.fields[fileName] =
                CommonFunctions.loadObjectFromJson<FieldInfo>(
                    $"Assets/Resources/Jsons/Fields/{fileName}"
                    );
        }
        LocalStorage.DICTIONARY_LOADING_DONE = true;

        foreach (string folderName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/TileSets/Tiers"
            ))
        {
            foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
                $"Assets/Resources/Jsons/TileSets/Tiers/{folderName}"
                ))
            {
                LocalDictionary.tileSets[Enum.Parse<TileSetNameEnum>(fileName)] =
                    CommonFunctions.loadObjectFromJson<TileSetInfo>(
                        $"Assets/Resources/Jsons/TileSets/Tiers/{folderName}/{fileName}"
                        );
            }
        }

        LocalStorage.monsters = CommonFunctions.loadObjectFromJson<List<MonsterVO>>(
            "Assets/Resources/Jsons/Save/Monsters"
            );
        LocalStorage.MONSTER_DATACALL_DONE = true;

        LocalStorage.tileSets = CommonFunctions.loadObjectFromJson<List<TileSetBriefInfo>>(
            "Assets/Resources/Jsons/Save/TileSets"
            );
        LocalStorage.TILESET_DATACALL_DONE = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
