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

        foreach (string folderName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Items"
            ))
        {
            foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            $"Assets/Resources/Jsons/Items/{folderName}"
            ))
            {
                LocalDictionary.items[fileName] =
                    CommonFunctions.loadObjectFromJson<ItemDictionaryVO>(
                        $"Assets/Resources/Jsons/Items/{folderName}/{fileName}"
                        );
            }
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
        LocalStorage.DataCall.DICTIONARY = true;

        LocalStorage.monsters = CommonFunctions.loadObjectFromJson<List<MonsterVO>>(
            "Assets/Resources/Jsons/Save/Monsters"
            );
        LocalStorage.DataCall.MONSTER = true;

        LocalStorage.items = CommonFunctions.loadObjectFromJson<List<ItemSaveVO>>(
            "Assets/Resources/Jsons/Save/Items"
            );

        foreach (ItemSaveVO itemSave in LocalStorage.items)
        {
            itemSave.setDetailedInfo(LocalDictionary.items[itemSave.itemName]);
        }
        LocalStorage.DataCall.ITEM = true;

        LocalStorage.journeyInfo = CommonFunctions.loadObjectFromJson<List<int>>(
            "Assets/Resources/Jsons/Save/Journey"
            );
        LocalStorage.DataCall.JOURNEY = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
