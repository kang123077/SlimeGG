using System.Collections;
using System.Collections.Generic;

public static class SettingVariables
{
    public static float slideToggleSpd = 0.25f;
    public static class Battle
    {
        // 0번 칸 : 열 개수 = x 좌표
        // 1번 칸 : 행 개수 = y 좌표
        public static int[] entrySizeMax = new int[] { 4, 7 };
    }
    public static class Reward
    {
        public static Dictionary<int, float[]> tierRandomStandard = new Dictionary<int, float[]>()
        {
            { 0, new float[4] { 0.7f, 1f, 2f, 2f } },
            { 1, new float[4] { 0.7f, 1f, 2f, 2f } },
            { 2, new float[4] { 0.7f, 1f, 2f, 2f } },
            { 3, new float[4] { 0.7f, 1f, 2f, 2f } },
        };
        public static Dictionary<int, int> tierNumOfReward = new Dictionary<int, int>() {
            { 0, 3},
            { 1, 5},
            { 2, 7},
        };

        public static class RerollCount
        {
            public static int monster = 2;
            public static int item = 2;
        }
    }
}
