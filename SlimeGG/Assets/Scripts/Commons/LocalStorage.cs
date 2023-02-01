using System.Collections.Generic;
using UnityEngine;

public static class LocalStorage
{
    public static class DataCall
    {
        public static bool DICTIONARY = false;
        public static bool MONSTER = false;
        public static bool JOURNEY = false;
    }

    public static List<MonsterVO> monsters = new List<MonsterVO>();

    public static List<Transform> tileSetTransforms = new List<Transform>();

    public static bool IS_GAME_PAUSE = true;
    public static bool IS_BATTLE_FINISH = false;

    public static bool IS_SCENE_FADE_IN = true;
    public static bool IS_CAMERA_FREE = false;


    public static List<int> journeyInfo = new List<int>();
    
    public static class Inventory
    {
        public static List<SlotController> monster = new List<SlotController>();
        public static List<SlotController> equipment = new List<SlotController>();
        public static List<SlotController> item = new List<SlotController>();
    }
}