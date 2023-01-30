using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    StageController[] nextStageList;
    SpriteRenderer bgSprite;
    Transform xMarkTf;
    Transform lineListTf;
    [SerializeField]
    Transform linePrefab;
    Transform[] lineList;

    DungeonManager dungeonManager;

    private bool isClear { get; set; }
    private bool isClick = false;
    private bool isAccessible = false;

    [SerializeField]
    StageType stageType;

    // Start is called before the first frame update
    void Start()
    {
        bgSprite = GetComponent<SpriteRenderer>();
        bgSprite.color = colorPicker(stageType);
        lineListTf = transform.Find("Lines");
        xMarkTf = transform.Find("X Mark");

        lineList = new Transform[nextStageList.Length];
        for (int i = 0; i < lineList.Length; i++)
        {
            Transform newLineTf = Instantiate(linePrefab);
            newLineTf.SetParent(lineListTf);
            newLineTf.localPosition = Vector3.zero;
            newLineTf.GetComponent<LineRenderer>().endColor = colorPicker(nextStageList[i].stageType);
            newLineTf.GetComponent<LineRenderer>().SetPositions(new Vector3[]
            {
                (nextStageList[i].transform.localPosition - transform.localPosition) * 0.1f,
                (nextStageList[i].transform.localPosition - transform.localPosition) * 0.9f
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear)
        {
            bgSprite.color = new Color(0.4f, 0.4f, 0.4f, 1);
            isClear = false;
        }
    }

    static Color colorPicker(StageType stageType)
    {
        Color curColor = new Color();
        switch (stageType)
        {
            case StageType.Normal:
                curColor = new Color(1f, 0.81f, 0.25f, 1f);
                break;
            case StageType.Hard:
                curColor = new Color(1f, 0.55f, 0.25f, 1f);
                break;
            case StageType.Boss:
                curColor = new Color(0.77f, 0.09f, 0.09f, 1f);
                break;
            case StageType.Event:
                curColor = new Color(0.25f, 1f, 0.62f, 1f);
                break;
            case StageType.Shop:
                curColor = new Color(0.91f, 0.53f, 1f, 1f);
                break;
        }
        return curColor;
    }

    private void OnMouseUp()
    {
        if (isClick)
        {
            if (isAccessible)
            {
                Debug.Log("Accessible!");
                dungeonManager.enterStage(this);
            }
            else
            {
                Debug.Log("InAccessible!");
            }
        }
        isClick = false;
    }

    private void OnMouseEnter()
    {
        isClick = true;
    }

    private void OnMouseExit()
    {
        isClick = false;
    }

    public void setDungeonManager(DungeonManager dungeonManager)
    {
        this.dungeonManager = dungeonManager;
        foreach (StageController nextStage in nextStageList)
        {
            nextStage.setDungeonManager(dungeonManager);
        }
    }

    public void setIsAccessible(bool isAccessible)
    {
        this.isAccessible = isAccessible;
    }

    public void closeAccessNext()
    {
        foreach (StageController nextStage in nextStageList)
        {
            nextStage.setIsAccessible(false);
        }
    }
    public void openAccessNext()
    {
        isClear = true;
        foreach (StageController nextStage in nextStageList)
        {
            nextStage.setIsAccessible(true);
        }
    }

    public void clearStage()
    {
        isClear = true;
        if (xMarkTf == null)
        {
            xMarkTf = transform.Find("X Mark");
        }
        xMarkTf.gameObject.SetActive(true);
    }
}
