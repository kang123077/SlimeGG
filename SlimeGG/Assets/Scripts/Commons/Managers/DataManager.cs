using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
        $"Assets/Resources/Jsons/Monsters"
        ))
        {
            LocalDictionary.speices[fileName] =
                CommonFunctions.loadObjectFromJson<MonsterDictionaryStat>(
                    $"Assets/Resources/Jsons/Monsters/{fileName}"
                    );
        }

        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Skills"
            ))
        {
            LocalDictionary.skills[fileName] =
                CommonFunctions.loadObjectFromJson<SkillStat>(
                    $"Assets/Resources/Jsons/Skills/{fileName}"
                    );
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
                    CommonFunctions.loadObjectFromJson<ItemDictionaryStat>(
                        $"Assets/Resources/Jsons/Items/{folderName}/{fileName}"
                        );
            }
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

        foreach (MonsterSaveStat monsterSaveStat in CommonFunctions.loadObjectFromJson<List<MonsterSaveStat>>(
            "Assets/Resources/Jsons/Save/Monsters"
            ))
        {
            // 각 저장된 몬스터 데이터 기반으로 Live데이터 만들기
            GeneratorFunction.generateMonsterLiveStat(monsterSaveStat);
        }
        LocalStorage.DataCall.MONSTER = true;

        foreach (ItemSaveStat itemSaveStat in CommonFunctions.loadObjectFromJson<List<ItemSaveStat>>(
            "Assets/Resources/Jsons/Save/Items"
            ))
        {
            // 각 저장된 아이템 기반으로 Live데이터 만들기
            GeneratorFunction.generateItemLiveStat(itemSaveStat);
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
