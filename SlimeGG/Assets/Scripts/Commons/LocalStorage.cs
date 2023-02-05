using System.Collections.Generic;
using UnityEngine;

public static class LocalStorage
{
    public static class DataCall
    {
        public static bool DICTIONARY = false;
        public static bool MONSTER = false;
        public static bool JOURNEY = false;
        public static bool ITEM = false;
    }

    public static class Live
    {
        public static Dictionary<string, MonsterLiveStat> monsters = new Dictionary<string, MonsterLiveStat>();
        public static Dictionary<string, ItemLiveStat> items = new Dictionary<string, ItemLiveStat>();
    }

    public static List<Transform> tileSetTransforms = new List<Transform>();

    public static bool IS_GAME_PAUSE = true;
    public static bool IS_BATTLE_FINISH = false;

    public static bool IS_SCENE_FADE_IN = true;
    public static bool IS_CAMERA_FREE = false;
    public static bool IS_CAMERA_FIX = false;


    public static List<int> journeyInfo = new List<int>();

    public static Dictionary<string, List<SlotController>> inventory = new Dictionary<string, List<SlotController>>()
    {
        { "monsters", new List<SlotController>() },
        { "equipments", new List<SlotController>() },
        { "items", new List<SlotController>() },
    };
}