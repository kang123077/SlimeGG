public static class PathInfo
{
    public static string JSON = "Jsons/";
    private static string sprite = "Sprites/";
    private static string animation = "Animations/";
    public static class Monster
    {
        private static string root = "Monsters/";
        public static string Sprite = $"{sprite}{root}";
        public static string Animation = $"{animation}{root}";
    }
    public static class Skill
    {
        private static string root = "Skills/";
        public static string Sprite = $"{sprite}{root}";
        public static string Animation = $"{animation}{root}";
    }

    public static class Effect
    {
        private static string root = "Effects/";
        public static class Area
        {
            private static string root = "Areas/";
            public static string Sprite = $"{sprite}{Effect.root}{root}";
            public static string Animation = $"{animation}{Effect.root}{root}";
        }
        public static class Attack
        {
            private static string root = "Attacks/";
            public static string Sprite = $"{sprite}{Effect.root}{root}";
            public static string Animation = $"{animation}{Effect.root}{root}";
        }
        public static class Casting
        {
            private static string root = "Castings/";
            public static string Sprite = $"{sprite}{Effect.root}{root}";
            public static string Animation = $"{animation}{Effect.root}{root}";
        }
    }

    public static class Item
    {
        private static string root = "Items/";
        public static string Sprite = $"{sprite}{root}";
    }

    public static class Json
    {
        private static string root = $"Jsons/";
        public static class Save
        {
            private static string root = $"Save/";
            public static string monster = $"{Json.root}{root}Monsters";
            public static string journey = $"{Json.root}{root}Journey";
            public static string item = $"{Json.root}{root}Items";
            public static string currency = $"{Json.root}{root}Currency";
        }

        public static class Dictionary
        {
            public static string Dungeon = $"{Json.root}Dungeons/";
            public static string Field = $"{Json.root}Fields/";
        }
    }

    public static string TEXTURE = "Textures/";
}
