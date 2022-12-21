using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupUIController : MonoBehaviour
{
    public TMP_Text headerUI;
    public TMP_Text infoUI;

    public void generateUI(TileSetInfo tileSetInfo)
    {
        headerUI.text = string.Format($"TileSet");
        infoUI.text =
            string.Format(
            $"TileName : {tileSetInfo.tileShape}\n" +
            $"Terrain : {tileSetInfo.tileType}"
            );
    }

    public void generateUI(MonsterInfo monsterInfo)
    {
        headerUI.text = string.Format($"Monster");
        infoUI.text =
            string.Format(
            $"NickName : {monsterInfo.nickName}\n" +
            $"Species : {monsterInfo.accuSpecies[-1]}\n" +
            $"Home : Volcano"
            );
    }

    private void deleteBeforeUI()
    {
        if (infoUI)
        {
            Destroy(infoUI);
        }
    }
}
