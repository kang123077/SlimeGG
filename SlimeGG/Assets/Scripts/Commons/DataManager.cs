using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // 몬스터 정보 불러오기
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
                    MonsterSpeciesInfo tempMM =
                        CommonFunctions.loadObjectFromJson<MonsterSpeciesInfo>(
                            $"Assets/Resources/Jsons/Monsters/{folderName}/{fileName}"
                            );
                    tempMM.extractInfo();
                    LocalDictionary.monsters[Enum.Parse<MonsterSpeciesEnum>(fileName)] = tempMM;
                }
            }
        }

        // 타일 형태 별 좌표 정보 불러오기
        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/TileSets/Shapes"
            ))
        {
            LocalDictionary.tileSetCoors[Enum.Parse<TileSetShapeEnum>(fileName)] =
                CommonFunctions.loadObjectFromJson<TileSetShapeStat>(
                    "Assets/Resources/Jsons/TileSets/Shapes/" + fileName
                    );
        }

        /** 스킬 별 정보 불러오기::
         */
        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Monsters/Skills"
            ))
        {
            LocalDictionary.skills[Enum.Parse<MonsterSkillEnum>(fileName)] =
                CommonFunctions.loadObjectFromJson<SkillStat>(
                    $"Assets/Resources/Jsons/Monsters/Skills/{fileName}"
                    );
        }

        /** 필드 별 정보 불러오기::
         */
        foreach (string fileName in CommonFunctions.loadFileNamesFromFolder(
            "Assets/Resources/Jsons/Fields"
            ))
        {
            LocalDictionary.fields[Enum.Parse<FieldNameEnum>(fileName)] =
                CommonFunctions.loadObjectFromJson<FieldInfo>(
                    $"Assets/Resources/Jsons/Fields/{fileName}"
                    );
        }

        LocalStorage.DICTIONARY_LOADING_DONE = true;

        /** 타일 종류 별 정보 불러오기::
         *      타일 형태
         *      타일 티어
         *      타일 종류
         *      타일 디스플레이되는 이름
         *      타일 속성
         */
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

        // 세이브 정보 불러오기
        //      몬스터4
        List<MonsterInfo> temp = CommonFunctions.loadObjectFromJson<List<MonsterInfo>>(
            "Assets/Resources/Jsons/Save/Monsters"
            );
        foreach(MonsterInfo tempM in temp)
        {
            tempM.extractInfo();
            LocalStorage.monsters.Add(tempM);
        }
        LocalStorage.MONSTER_DATACALL_DONE = true;

        //      타일셋
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
