using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
        $"Assets/Resources/Jsons/Monsters"
        ))
        {
            MonsterDictionaryStat stat =
                CommonFunctions.loadObjectFromJson<MonsterDictionaryStat>(
                    $"Assets/Resources/Jsons/Monsters/{fileName}"
                    );
            LocalDictionary.speicies[fileName] = stat;
            LocalDictionary.speicesByTier[stat.tier].Add(stat);
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

        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
        $"Assets/Resources/Jsons/Items"
        ))
        {
            ItemDictionaryStat stat =
                CommonFunctions.loadObjectFromJson<ItemDictionaryStat>(
                    $"Assets/Resources/Jsons/Items/{fileName}"
                    );
            LocalDictionary.items[fileName] = stat;
            LocalDictionary.itemsByTier[stat.tier].Add(stat);
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

        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Dungeons"
            ))
        {
            LocalDictionary.dungeons[fileName] =
                CommonFunctions.loadObjectFromJson<List<StageSaveStat>>(
                    $"Assets/Resources/Jsons/Dungeons/{fileName}"
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

        LocalStorage.Live.journeyInfo = CommonFunctions.loadObjectFromJson<List<int>>(
            "Assets/Resources/Jsons/Save/Journey"
            );

        LocalStorage.Live.dungeonHistory = CommonFunctions.loadObjectFromJson<List<string>>(
            "Assets/Resources/Jsons/Save/DungeonHistory"
            );
        LocalStorage.DataCall.JOURNEY = true;
        
        // 만약 journeyInfo가 비었다? -> 월드라는 소림
        if (LocalStorage.Live.journeyInfo.Count != 0)
        {
            LocalStorage.CurrentLocation.dungeonName = LocalStorage.Live.dungeonHistory[LocalStorage.Live.dungeonHistory.Count - 1];
        } else
        {

        }

        LocalStorage.Live.currency = CommonFunctions.loadObjectFromJson<int>(
            "Assets/Resources/Jsons/Save/Currency"
            );
        LocalStorage.DataCall.CURRENCY = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
