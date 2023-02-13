using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[JsonConverter(typeof(StringEnumConverter))]
public enum ElementEnum
{
    Fire, // 불
    Water, // 물
    Grass, // 풀

    Animal, // 짐승
    Spirit, // 정령 (Elements?)
    Machine, // 기계

    Wind, // 바람
    Earth, // 대지 (Stone?)
    Electric, // 전기 ★

    Light, // 빛 ★
    Dark, // 어둠 ★

    Devil, // 악마 (Demon?)
    Angel, // 천사
    Dragon, // 용 ★
}