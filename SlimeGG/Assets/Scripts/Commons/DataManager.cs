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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
