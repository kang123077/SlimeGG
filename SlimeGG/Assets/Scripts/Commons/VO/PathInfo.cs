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
    public static string TEXTURE = "Textures/";
}
