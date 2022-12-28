using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PopupUIController : MonoBehaviour
{
    public TMP_Text headerUI;
    public TMP_Text infoUI;

    public void generateUI(TileSetInfo tileSetInfo)
    {
        headerUI.text = string.Format($"TileSet");
        //infoUI.text =
        //    string.Format(
        //    $"TileName : {tileSetInfo.tileShape}\n" +
        //    $"Terrain : {tileSetInfo.tileType}"
        //    );
    }

    public void generateUI(MonsterInfo monsterInfo, Transform curTileSet)
    {
        headerUI.text = string.Format($"Monster");
        if (curTileSet == null)
        {
            infoUI.text =
                string.Format(
                $"NickName : {monsterInfo.nickName}\n" +
                // $"Stage : {}\n" +
                $"Species : {monsterInfo.accuSpecies.Last()}\n" +
                $"\n" +
                $"HP : {monsterInfo.hp}\n" +
                $"DEF : {monsterInfo.def}\n" +
                $"SPD : {monsterInfo.spd}\n" +
                $"\n" +
                $"Elements\n" +
                $"{monsterInfo.elements[0]} : " +
                $"{monsterInfo.stats[0]}\n" +
                $"{monsterInfo.elements[1]} : " +
                $"{monsterInfo.stats[1]}\n" +
                $"{monsterInfo.elements[2]} : " +
                $"{monsterInfo.stats[2]}\n"
                );
        }
        else
        {
            infoUI.text =
                string.Format(
                $"NickName : {monsterInfo.nickName}\n" +
                // $"Stage : {}\n" +
                $"Species : {monsterInfo.accuSpecies.Last()}\n" +
                $"\n" +
                $"HP : {monsterInfo.hp}\n" +
                $"DEF : {monsterInfo.def}\n" +
                $"SPD : {monsterInfo.spd}\n" +
                $"\n" +
                $"Elements\n" +
                $"{monsterInfo.elements[0]} : " +
                $"{monsterInfo.stats[0]}\n" +
                $"{monsterInfo.elements[1]} : " +
                $"{monsterInfo.stats[1]}\n" +
                $"{monsterInfo.elements[2]} : " +
                $"{monsterInfo.stats[2]}\n" +
                $"\n" +
                $"HomeTile : {curTileSet.GetComponent<TileSetController>().getTileSetBriefInfoName()}\n" +
                $"Skills\n" +
                $""
                );
        }
    }

    private void deleteBeforeUI()
    {
        if (infoUI)
        {
            Destroy(infoUI);
        }
    }
}
