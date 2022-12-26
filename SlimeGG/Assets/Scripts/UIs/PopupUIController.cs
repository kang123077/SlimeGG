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
                $"Species : {monsterInfo.accuSpecies.Last()}\n"
                );
        }
        else
        {
            infoUI.text =
                string.Format(
                $"NickName : {monsterInfo.nickName}\n" +
                $"Species : {monsterInfo.accuSpecies.Last()}\n" +
                $"Home : {curTileSet.GetComponent<TileSetController>().getTileSetBriefInfoName()}\n"
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
