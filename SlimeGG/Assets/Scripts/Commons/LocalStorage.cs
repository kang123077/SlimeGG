using System.Collections.Generic;
using UnityEngine;

public static class LocalStorage
{
    public static bool DICTIONARY_LOADING_DONE = false;

    public static bool SOCKET_LOADING_DONE = false;
    public static bool TILESET_LOADING_DONE = false;
    public static bool MONSTER_LOADING_DONE = false;

    public static bool MONSTER_DATACALL_DONE = false;
    public static bool TILESET_DATACALL_DONE = false;

    public static bool BATTLE_SCENE_LOADING_DONE = false;

    public static List<MonsterInfo> monsters = new List<MonsterInfo>();
    public static List<TileSetBriefInfo> tileSets = new List<TileSetBriefInfo>();

    public static List<Transform> tileSetTransforms = new List<Transform>();

    public static bool IS_GAME_PAUSE = true;
}