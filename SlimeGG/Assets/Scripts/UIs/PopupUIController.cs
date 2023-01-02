using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

public class PopupUIController : MonoBehaviour
{
    public TMP_Text headerUI;
    public TMP_Text infoUI;
    public void generateUI(TileSetInfo tileSetInfo)
    {
        StringBuilder elementUI = new StringBuilder();
        for (int i = 0; i < tileSetInfo.elements.Count; i++)
        {
            elementUI.Append($"{tileSetInfo.elements[i]} : ");
            elementUI.Append($"{tileSetInfo.stats[i]}\n");
        }

        headerUI.text = string.Format($"TileSet");
        infoUI.text =
            string.Format($"Tilename : {tileSetInfo.tileName}\n" +
            $"Nickname : {tileSetInfo.nickName}\n" +
            $"Tier : {tileSetInfo.tier}\n" +
            $"\n" +
            $"Elements\n" +
            $"{elementUI}"
            );
    }

    public void generateUI(MonsterInfo monsterInfo, Transform curTileSet, MonsterSpeciesInfo speciesInfo)
    {
        StringBuilder elementUI = new StringBuilder();
        for (int i = 0; i < monsterInfo.elements.Count; i++)
        {
            elementUI.Append($"{monsterInfo.elements[i]} : ");
            elementUI.Append($"{Mathf.Round(monsterInfo.stats[i] * 10) / 10}\n");
        }

        StringBuilder skillUI = new StringBuilder();
        for (int i = 0; i < speciesInfo.skills.Count; i++)
        {
            skillUI.Append($"{speciesInfo.skills[i]}\n");
        }

        headerUI.text = string.Format($"Monster");

        infoUI.text =
            string.Format(
            $"NickName : {monsterInfo.nickName}\n" +
            $"Stage : {speciesInfo.stage}\n" +
            $"Species : {monsterInfo.accuSpecies.Last()}\n" +
            $"\n" +
            $"HP : {monsterInfo.hp}\n" +
            $"DEF : {monsterInfo.def}\n" +
            $"SPD : {monsterInfo.spd}\n" +
            $"HomeTile : {curTileSet.GetComponent<TileSetController>().getTileSetBriefInfoName()}\n" +
            $"\n" +
            $"Elements\n" +
            $"{elementUI}" +
            $"\n" +
            $"Skills\n" +
            $"{skillUI}"
            );
    }
}
