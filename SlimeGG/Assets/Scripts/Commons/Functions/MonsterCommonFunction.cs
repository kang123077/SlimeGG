using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterCommonFunction
{

    // 몬스터 종족 정보 + 아이템 정보 -> 전투에 필요한 몬스터 객체 생성
    public static MonsterBattleInfo generateMonsterBattleInfo(MonsterLiveStat monsterLiveStat)
    {
        MonsterBattleInfo res = new MonsterBattleInfo();
        res.speicie = monsterLiveStat.saveStat.speicie;
        // 기본 정보는 곱연산을 통한 설정
        // 속성 정보는 합연산을 통한 설정
        
        // 종족의 기본 정보 불러오기
        foreach (BasicStat basicStat in monsterLiveStat.dictionaryStat.basic)
        {
            res.basic[basicStat.name] = new BasicStat(basicStat.amount);
        }

        // 아이템 효과들 적용
        foreach (ItemLiveStat itemStat in monsterLiveStat.itemStatList.Values)
        {
            // item의 effect 갯수만큼
            for (int i = 0; i < itemStat.dictionaryStat.effect.Count; i++)
            {
                // isMultiple이 false(더하기)면
                if (itemStat.dictionaryStat.effect[i].isMultiple == false)
                    if (res.basic.ContainsKey(itemStat.dictionaryStat.effect[i].name))
                    {
                        res.basic[itemStat.dictionaryStat.effect[i].name].amount += itemStat.dictionaryStat.effect[i].amount;
                    } else
                    {
                        res.basic[itemStat.dictionaryStat.effect[i].name] = new BasicStat(itemStat.dictionaryStat.effect[i].amount);
                    }
                // isMultiple이 true(곱하기)면
                else
                if (res.basic.ContainsKey(itemStat.dictionaryStat.effect[i].name))
                {
                    res.basic[itemStat.dictionaryStat.effect[i].name].amount *= itemStat.dictionaryStat.effect[i].amount;
                }
                else
                {
                    res.basic[itemStat.dictionaryStat.effect[i].name] = new BasicStat(0);
                }
            }
        }

        // 전투시 속성 불러오기
        res.element = monsterLiveStat.dictionaryStat.element;

        // 스킬 정보 불러오기
        foreach (string skillName in monsterLiveStat.dictionaryStat.skills)
        {
            res.skills[skillName] = new SkillStat(LocalDictionary.skills[skillName]);
        }

        // 전투 시에 필요한 추가 설정치
        res.basic[BasicStatEnum.position] = new BasicStat(0f);
        res.basic[BasicStatEnum.invincible] = new BasicStat(0f);
        res.entryPos = monsterLiveStat.saveStat.entryPos != null ? monsterLiveStat.saveStat.entryPos : new int[2];
        return res;
    }

    public static Vector3 translatePositionIntoVector3(float amount, MonsterBattleController caster, MonsterBattleController target)
    {
        Vector3 res = new Vector3();
        return res;
    }

    public static int decideDistortion(Vector3 direction, Vector3 location)
    {
        int res = 0;
        float d = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z; ;
        if (325f <= d || d < 45f)
        {
            res = location.x >= 0f ? -1 : 1;
        }
        else if (45f <= d && d < 135f)
        {
            res = location.y >= 0f ? -1 : 1;
        }
        else if (135f <= d && d < 225f)
        {
            res = location.x >= 0f ? 1 : -1;
        }
        else if (225f <= d && d < 325f)
        {
            res = location.y >= 0f ? 1 : -1;
        }
        return res;
    }

    public static Vector3 getDistortedDirection(Vector3 direction, Vector3 location, int distortionDirection)
    {
        if (distortionDirection == 0) return direction;
        float heightTop = (BattleManager.fieldSize.y - 4f) / 2;
        float widthTop = (BattleManager.fieldSize.x - 4f) / 2;
        float hardness = 0f;
        Vector3 res = Vector3.Normalize(Quaternion.AngleAxis(distortionDirection == 1 ? -90 : 90, Vector3.forward) * direction) / 10f;
        float d = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
        if (325f <= d || d < 45f)
        {
            hardness = (heightTop + location.y) / (heightTop - location.y);
        }
        else if (45f <= d && d < 135f)
        {
            hardness = (widthTop - location.x) / (widthTop + location.x);
        }
        else if (135f <= d && d < 225f)
        {
            hardness = (heightTop - location.y) / (heightTop + location.y);
        }
        else if (225f <= d && d < 325f)
        {
            hardness = (widthTop + location.x) / (widthTop - location.x);
        }
        res *= hardness;
        return res;
    }

    public static Vector3 translatePositionPowerToVector3(Vector3 direction, float power)
    {
        return direction * power * 10;
    }
}
