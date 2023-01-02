using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterCommonFunction
{
    // 몬스터 정보 + 몬스터 종족 정보 -> 전투에 필요한 몬스터 객체 생성
    public static MonsterBattleInfo generateMonsterBattleInfo(MonsterVO monsterVO, MonsterSpeciesVO speciesVO)
    {
        MonsterBattleInfo res = new MonsterBattleInfo();
        res.nickName = monsterVO.nickName;
        res.src = speciesVO.src;
        // 기본 정보는 곱연산을 통한 설정
        // 속성 정보는 합연산을 통한 설정
        foreach (BasicStatVO basicStat in monsterVO.basic)
        {
            res.basic[basicStat.name] = new PlainStatVO(basicStat.amount);
        }
        foreach (BasicStatVO basicStat in speciesVO.basic)
        {
            res.basic[basicStat.name].amount *= basicStat.amount;
        }
        foreach (ElementStatVO elementStat in monsterVO.element)
        {
            res.element[elementStat.name] = new PlainStatVO(elementStat.amount);
        }
        foreach (ElementStatVO elementStat in speciesVO.element)
        {
            if (res.element.Keys.Contains(elementStat.name))
                res.element[elementStat.name].amount += elementStat.amount;
            else
                res.element[elementStat.name] = new PlainStatVO(elementStat.amount);
        }
        foreach (string skillName in speciesVO.skills)
        {
            res.skills[skillName] = LocalDictionary.skills[skillName];
        }
        // 전투 시에 필요한 추가 설정치
        res.basic[BasicStatEnum.position] = new PlainStatVO(0f);
        res.basic[BasicStatEnum.invincible] = new PlainStatVO(0f);
        res.basic[BasicStatEnum.timeCoolCycle] = new PlainStatVO(0f);
        res.basic[BasicStatEnum.timeMoveCycle] = new PlainStatVO(0f);
        return res;
    }

    public static MonsterFarmInfo generateMonsterFarmInfo(MonsterVO monsterVO, MonsterSpeciesVO speciesVO)
    {
        MonsterFarmInfo res = new MonsterFarmInfo();
        return res;
    }

    public static Vector3 translatePositionIntoVector3(float amount, MonsterBattleController caster, MonsterBattleController target)
    {
        Vector3 res = new Vector3();
        return res;
    }
}
