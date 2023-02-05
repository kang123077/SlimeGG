using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AttributesFunction
{
    private static ElementEnum[][] pairList = new ElementEnum[][]
    {
        new ElementEnum[] { ElementEnum.Fire, ElementEnum.Water, ElementEnum.Grass },
        new ElementEnum[] { ElementEnum.Animal, ElementEnum.Spirit, ElementEnum.Machine },
        new ElementEnum[] { ElementEnum.Wind, ElementEnum.Earth, ElementEnum.Electric },
        new ElementEnum[] { ElementEnum.Devil, ElementEnum.Angel, ElementEnum.Dragon },
        new ElementEnum[] { ElementEnum.Light, ElementEnum.Dark },
    };

    private static float weightInSuperior = 0.25f;
    private static float weightInInferior = -0.25f;

    public static float calculateAttributeAgainstWeight(
        List<ElementEnum> atkElList,
        List<ElementEnum> defElList)
    {
        // 서로 같은 가위바위보 안에 있는 속성끼리 매칭
        float res = 1f;

        List<ElementEnum> tempElList;
        float temp;
        while (atkElList.Count > 0)
        {
            temp = 1f;
            tempElList = defElList.ToList();
            while (tempElList.Count > 0)
            {
                if ((temp = getAttributeWeight(atkElList[0], tempElList[0])) != 1f)
                {
                    // 현재 두 원소간 상성이 존재할 경우
                    res += temp;
                    atkElList.RemoveAt(0);
                    break;
                }
                else
                {
                    // 현재 두 원소간 상성이 존재하지 않을 경우
                    tempElList.RemoveAt(0);
                }
            }
            if (atkElList.Count == 0) break;
            atkElList.RemoveAt(0);
        }
        return res;
    }

    private static float getAttributeWeight(ElementEnum atkEl, ElementEnum defEl)
    {
        if (atkEl == defEl) return 1f;
        int atkElIdx;
        int defElIdx;
        for (int i = 0; i < pairList.Length; i++)
        {
            if ((atkElIdx = Array.FindIndex(pairList[i], (el) =>
            {
                return el == atkEl;
            })) != -1)
            {
                if ((defElIdx = Array.FindIndex(pairList[i], (el) =>
                {
                    return el == defEl;
                })) != -1)
                {
                    // 같은 페어일 경우
                    if (atkEl == ElementEnum.Devil || atkEl == ElementEnum.Angel) return weightInSuperior;
                    switch (atkEl - defEl)
                    {
                        case 1:
                        case -2:
                            return weightInSuperior;
                        case -1:
                        case 2:
                            return weightInInferior;
                    }
                }
                return 1f;
            }
        }
        return 1f;
    }
}
