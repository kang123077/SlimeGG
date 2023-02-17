using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventController : ObjectSizeController
{
    private Button buttonMain;
    private Image imageBackground, imageLeft, imageRight;
    private TextMeshProUGUI textDesc;
    private Transform choiceBoxTf;
    private int curStatus = 0;
    private Button[] choiceButtons = new Button[3];
    private TextMeshProUGUI[] choiceTexts = new TextMeshProUGUI[3];
    private EventSaveStat[] pages = new EventSaveStat[50];
    private ObjectMoveController moveController;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
        switch (curStatus)
        {
            case 0:
                // 객체 연결
                initSetting();
                break;
            case 1:
                // 정보 불러오기
                loadEvent();
                // 최초 이벤트 페이지 연결
                goToPage(0);
                curStatus = 2;
                break;
            case 2:
                // 이벤트 실행중
                break;
        }
    }

    private void initSetting()
    {
        moveController = GetComponent<ObjectMoveController>();
        imageBackground = transform.GetChild(0).GetComponent<Image>();
        imageLeft = transform.GetChild(1).GetComponent<Image>();
        imageRight = transform.GetChild(2).GetComponent<Image>();
        Transform temp = transform.GetChild(3);
        buttonMain = temp.GetComponent<Button>();
        textDesc = temp.GetChild(0).GetComponent<TextMeshProUGUI>();
        choiceBoxTf = temp.GetChild(1);
        Transform choiceTf;
        for (int i = 0; i < 3; i++)
        {
            choiceTf = choiceBoxTf.GetChild(i);
            choiceButtons[i] = choiceTf.GetComponent<Button>();
            choiceTexts[i] = choiceTf.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        curStatus = 1;
    }

    private void loadEvent()
    {
        foreach (EventSaveStat page in CommonFunctions.loadObjectFromJson<List<EventSaveStat>>(
                    $"Assets/Resources/Jsons/Events/Test"
                    ))
        {
            pages[page.id] = page;
        }
    }

    private void goToPage(int idx)
    {
        EventSaveStat curPage = pages[idx];
        if (curPage.background != null)
            imageBackground.sprite = Resources.Load<Sprite>(
            PathInfo.Background.Sprite + curPage.background
            );
        else imageBackground.sprite = null;
        if (curPage.characterLeft != null)
            imageLeft.sprite = Resources.Load<Sprite>(
                PathInfo.Monster.Sprite + curPage.characterLeft
                );
        else imageLeft.sprite = null;
        if (curPage.characterRight != null)
            imageRight.sprite = Resources.Load<Sprite>(
            PathInfo.Monster.Sprite + curPage.characterRight
            );
        else imageRight.sprite = null;
        textDesc.text = curPage.desc;
        buttonMain.onClick.RemoveAllListeners();
        choiceBoxTf.gameObject.SetActive(false);

        if (curPage.choices == null && curPage.nextId == null)
        {
            // 다음 페이지 없음
            buttonMain.onClick.AddListener(() =>
            {
                // 리워드 적용 및 이벤트 종료
                applyReward(curPage.rewards);
                finishReward();
            });
            return;
        }
        // 다음 페이지 있음
        if (curPage.choices == null)
        {
            // 선택지 없음
            buttonMain.onClick.AddListener(() =>
            {
                // 다음 페이지로 이동
                goToPage((int)curPage.nextId);
            });
            return;
        }
        // 선택지 있음
        choiceBoxTf.gameObject.SetActive(true);
        int i = 0;
        foreach (EventChoiceStat choice in curPage.choices)
        {
            choiceTexts[i].text = choice.desc;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() =>
            {
                goToPage(choice.destId);
            });
            i++;
        }
    }

    private void applyReward(List<EventRewardStat> rewardStats)
    {
        foreach (EventRewardStat stat in rewardStats)
        {
            Debug.Log($"보상:: {(stat.isRandom ? "랜덤" : "일반")} | {(stat.type == 0 ? "몬스터" : stat.type == 1 ? "아이템" : "재화")} | {stat.name} | {(stat.isGive ? "추가" : "삭제")}");
        }
    }

    private void finishReward()
    {
        moveController.toggle();
    }

    public void openModule()
    {
        moveController.toggle();
    }
}
