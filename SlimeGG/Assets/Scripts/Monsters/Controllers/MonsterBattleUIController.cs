using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MonsterBattleUIController : MonoBehaviour
{
    private static List<Vector3> posList = new List<Vector3>() {
    new Vector3(150f, 100f, 0f),
    new Vector3(125f, 275f, 0f),
    new Vector3(375, 100f, 0f),
    new Vector3(125f, 450f, 0f)};
    private HorizontalLayoutGroup horizontalLayout;
    private MonsterBattleController target;
    private Transform trackingTf;
    private Dictionary<MonsterSkillEnum, Transform> skillTfList = new Dictionary<MonsterSkillEnum, Transform>();
    private bool isDead = false;

    void Start()
    {
        Transform tempTf = transform.Find("Contents");
        horizontalLayout = tempTf.GetComponent<HorizontalLayoutGroup>();
        trackingTf = tempTf.Find("Tracking");
    }

    void Update()
    {
        if (LocalStorage.BATTLE_SCENE_LOADING_DONE &&
            !LocalStorage.IS_BATTLE_FINISH &&
            !LocalStorage.IS_GAME_PAUSE)
        {
            updateCoolTime();
        }
    }

    private void reverseTrackingAndSkills()
    {
        Vector3 res = transform.localScale.x == 1f ? new Vector3(-1, 1, 1) : Vector3.one;
        transform.localScale = res;
        Transform temp = transform.Find("Contents").Find("Skills");
        for (int i = 1; i <= 3; i++)
        {
            temp.Find($"{i}").Find("CoolTime").localScale = res;
        }
    }

    public void initInfo(MonsterBattleController target, int side, int numPos)
    {
        transform.localScale = Vector3.one;
        Transform tempTf = transform.Find("Contents").Find("Skills");
        this.target = target;
        int affordable = 1;
        foreach (SkillStat skillStat in target.skillStatList)
        {
            skillTfList[skillStat.skillName] = tempTf.Find($"{affordable}");
            affordable++;
        }
        if (side == 1)
        {
            reverseTrackingAndSkills();
        }
        // 위치 잡기
        Vector3 tragetPos = posList[numPos];
        transform.localPosition = new Vector3(side == 0 ? tragetPos.x : -tragetPos.x, tragetPos.y, tragetPos.z);

    }

    private void updateCoolTime()
    {
        if (target.isDead)
        {
            foreach (SkillStat skillStat in target.skillStatList)
            {
                skillTfList[skillStat.skillName].Find("CoolTime").GetComponent<Image>().type = Image.Type.Simple;
            }
            isDead = true;
        }
        foreach (SkillStat skillStat in target.skillStatList)
        {
            skillTfList[skillStat.skillName].Find("CoolTime").GetComponent<Image>().fillAmount = 
                1 - (target.skillTimer[skillStat.skillName] / skillStat.coolTime);
        }
    }
}
