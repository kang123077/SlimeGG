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
            $"Name : Monster 1\n" +
            $"Species : Purplime\n" +
            $"Age : 3d 2h 13m\n" +
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