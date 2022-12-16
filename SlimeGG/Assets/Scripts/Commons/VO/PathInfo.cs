using System.Collections.Generic;

public static class PathInfo
{
    public static string PATH_ANIMATION = "Animations/";
    public static string PATH_SPRITE = "Sprites/";
    public static class Monster
    {
        public static string PATH = "Monsters/";
        public static string PATH_EGG = Monster.PATH + "Eggs/";
        public static string PATH_INFANT = Monster.PATH + "Infants/";
        public static string PATH_LV2 = Monster.PATH + "Lv2s/";
        public static string PATH_LV3 = Monster.PATH + "Lv3s/";
        public static string PATH_LV4 = Monster.PATH + "Lv4s/";

        public static Dictionary<GrowthState, Dictionary<string, string>> paths = new Dictionary<GrowthState, Dictionary<string, string>>()
            {
                {
                    GrowthState.Egg, new Dictionary<string, string>()
                    {
                        {"Normal", PATH_EGG + "Normal"},
                    }
                },
                {
                    GrowthState.Infant, new Dictionary<string, string>()
                    {
                        {"Ore", PATH_INFANT + "Ore"},
                    }
                },
                {
                    GrowthState.Lv2, new Dictionary<string, string>()
                    {

                    }
                },
                {
                    GrowthState.Lv3, new Dictionary<string, string>()
                    {

                    }
                },
                {
                    GrowthState.Lv4, new Dictionary<string, string>()
                    {

                    }
                },
            };
    }
}
