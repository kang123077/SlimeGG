using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;

public class MonsterBattleUIController : MonoBehaviour
{
    private static List<Vector3> posList = new List<Vector3>() {
    new Vector3(150f, 100f, 0f),
    new Vector3(125f, 275f, 0f),
    new Vector3(375, 100f, 0f),
    new Vector3(125f, 450f, 0f)};
    private MonsterBattleController target;
    private Transform trackingTf;
    private Dictionary<string, Transform> skillTfList = new Dictionary<string, Transform>();
    private bool isDead = false;

    void Start()
    {
    }

    void Update()
    {
        if (BattleManager.isBattleReady &&
            !LocalStorage.IS_BATTLE_FINISH &&
            !LocalStorage.IS_GAME_PAUSE &&
            !isDead)
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
        Transform tempTf = transform.Find("Contents");
        trackingTf = tempTf.Find("Tracking");
        transform.localScale = Vector3.one;
        tempTf = tempTf.Find("Skills");
        this.target = target;
        int affordable = 1;
        foreach (KeyValuePair<string, SkillStat> skillStat in target.monsterBattleInfo.skills)
        {
            skillTfList[skillStat.Key] = tempTf.Find($"{affordable}");
            affordable++;
        }
        if (side == 1)
        {
            reverseTrackingAndSkills();
        }
        // 위치 잡기
        Vector3 tragetPos = posList[numPos];
        transform.localPosition = new Vector3(side == 0 ? tragetPos.x : -tragetPos.x, tragetPos.y, tragetPos.z);
        setTextureToTracking(side, numPos);

    }

    private void updateCoolTime()
    {
        if (target.isDead)
        {
            trackingTf.GetComponent<RawImage>().color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
            foreach (KeyValuePair<string, SkillStat> skillStat in target.monsterBattleInfo.skills)
            {
                skillTfList[skillStat.Key].Find("CoolTime").GetComponent<Image>().type = Image.Type.Simple;
            }
            isDead = true;
        }
        foreach (KeyValuePair<string, SkillStat> skillStat in target.monsterBattleInfo.skills)
        {
            skillTfList[skillStat.Key].Find("CoolTime").GetComponent<Image>().fillAmount = 
                1 - Mathf.Min((skillStat.Value.timeCharging / skillStat.Value.coolTime), 1f);
        }
    }

    private void setTextureToTracking(int side, int numPos)
    {
        trackingTf.GetComponent<RawImage>().texture = Resources.Load<RenderTexture>(
            PathInfo.TEXTURE + "MonsterTracking/" + $"{side}_{numPos}"
            );
    }
}