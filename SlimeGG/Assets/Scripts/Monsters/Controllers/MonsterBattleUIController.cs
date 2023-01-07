using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBattleUIController : MonoBehaviour
{
    private MonsterBattleController target;
    private Transform trackingTf;
    private Dictionary<string, Transform> skillTfList = new Dictionary<string, Transform>();
    private bool isDead = false;
    private int numPos { get; set; }

    void Start()
    {
    }

    void Update()
    {
        adjustSize();
        if (BattleManager.isBattleReady &&
            !LocalStorage.IS_BATTLE_FINISH &&
            !LocalStorage.IS_GAME_PAUSE &&
            !isDead)
        {
            updateCoolTime();
        }
    }

    private void adjustSize()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 6, Screen.width / 6 * 3 / 4);
        transform.localPosition = new Vector3(0, Screen.width / 6 * 3 / 4 * numPos, 0f);
    }

    private void reverseTrackingAndSkills()
    {
        Vector3 res = transform.localScale.x == 1f ? new Vector3(-1, 1, 1) : Vector3.one;
        trackingTf.localScale = res;
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
        trackingTf = tempTf.Find("Tracking").Find("Screen");
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
        this.numPos = numPos;
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