using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageController : MonoBehaviour
{
    StageController[] nextStageList;
    List<StageController> prevStageList = new List<StageController>();
    [SerializeField]
    Transform linePrefab;

    SpriteRenderer bgSprite;
    Transform xMarkTf;
    Transform lineListTf;
    Transform[] lineList;

    DungeonManager dungeonManager;

    private bool isClear { get; set; }
    private bool isClick = false;
    private bool isAccessible = false;
    private StageType stageType;
    private Vector3 locationPos;
    private int idx = -1;

    private int curStatus = 0;

    private List<int> nextIds = new List<int>();
    private bool isDrawing = false;

    // Start is called before the first frame update
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear)
        {
            bgSprite.color = new Color(0.4f, 0.4f, 0.4f, 1);
            isClear = false;
        }
        if (LocalStorage.EDITOR_MODE)
        {
            if (!isDrawing)
            {
                adjustPosition();
                adjustLine();
            }
        }
    }

    public void initInfo(StageSaveStat saveStat, List<StageController> nextStages = null, int idx = -1)
    {
        this.idx = idx;
        if (curStatus == 0)
        {
            initSetting();
        }
        stageType = saveStat.type;
        locationPos = new Vector3(
            saveStat.locationPos[0],
            saveStat.locationPos[1],
            0f);

        transform.localScale = Vector3.one * 0.5f;
        transform.localPosition = new Vector3(
            saveStat.locationPos[0],
            saveStat.locationPos[1],
            0f);
        nextStageList = nextStages != null ? nextStages.ToArray() : new StageController[0];
        foreach (StageController controller in nextStageList)
        {
            controller.addPrevStage(this);
        }
        drawLine();
    }

    public void initInfo(StageSaveStat saveStat, List<int> nextIds = null, int idx = -1)
    {
        this.idx = idx;
        if (curStatus == 0)
        {
            initSetting();
        }
        stageType = saveStat.type;
        locationPos = new Vector3(
            saveStat.locationPos[0],
            saveStat.locationPos[1],
            0f);

        transform.localScale = Vector3.one * 0.5f;
        transform.localPosition = new Vector3(
            saveStat.locationPos[0],
            saveStat.locationPos[1],
            0f);
        this.nextIds = nextIds;
    }

    public void initSetting()
    {
        bgSprite = GetComponent<SpriteRenderer>();
        bgSprite.color = colorPicker(stageType);
        lineListTf = transform.Find("Lines");
        xMarkTf = transform.Find("X Mark");
        curStatus = 1;
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
                LocalStorage.CurrentLocation.nodeNum = idx.ToString();
                LocalStorage.CurrentLocation.stageLevel = stageType == StageType.Normal ? 0 : stageType == StageType.Hard ? 1 : stageType == StageType.Boss ? 2 : 0;
                dungeonManager.enterStage(this);
            }
            else
            {
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

    public StageController getNextStage(int num)
    {
        return nextStageList[num];
    }

    public void addNextStage(StageController stageToAdd)
    {
        if (nextStageList == null) nextStageList = new StageController[0];
        if (stageToAdd == null || nextStageList.Contains(stageToAdd)) return;
        stageToAdd.addPrevStage(this);
        StageController[] newList = new StageController[(nextStageList != null ? nextStageList.Length : 0) + 1];
        int cnt = 0;
        if (nextStageList != null)
        {
            foreach (StageController stage in nextStageList)
            {
                if (stage != stageToAdd)
                    newList[cnt++] = stage;
            }
        }
        newList[cnt] = stageToAdd;
        nextStageList = newList;
        drawLine();
    }

    public void removeNextStage(StageController stageToRemove)
    {
        if (stageToRemove == null || !nextStageList.Contains(stageToRemove)) return;
        stageToRemove.removePrevStage(this);
        StageController[] newList = new StageController[(nextStageList != null ? nextStageList.Length : 0) - 1];
        int cnt = 0;
        if (nextStageList != null)
        {
            foreach (StageController stage in nextStageList)
            {
                if (stage != stageToRemove)
                    newList[cnt++] = stage;
            }
        }
        nextStageList = newList;
        drawLine();
    }

    private void drawLine()
    {
        isDrawing = true;
        if (lineList != null)
        {
            foreach (Transform line in lineList)
            {
                Destroy(line.gameObject);
            }
        }
        lineList = new Transform[nextStageList.Length];
        for (int i = 0; i < lineList.Length; i++)
        {
            Transform newLineTf = Instantiate(linePrefab);
            newLineTf.SetParent(lineListTf);
            newLineTf.localPosition = Vector3.zero;
            newLineTf.GetComponent<LineRenderer>().endColor = colorPicker(nextStageList[i].stageType);
            newLineTf.GetComponent<LineRenderer>().SetPositions(new Vector3[]
            {
                (nextStageList[i].locationPos - transform.localPosition) * 0.1f,
                (nextStageList[i].locationPos - transform.localPosition) * 0.9f
            });
            newLineTf.GetComponent<LineController>().fromStageController = this;
            newLineTf.GetComponent<LineController>().nextStageController = nextStageList[i];
            lineList[i] = newLineTf;
        }
        isDrawing = false;
    }

    private void adjustPosition()
    {
        locationPos = transform.position;
    }

    private void adjustLine()
    {
        if (lineList != null)
            for (int i = 0; i < lineList.Length; i++)
            {
                lineList[i].GetComponent<LineRenderer>().endColor = colorPicker(nextStageList[i].stageType);
                lineList[i].GetComponent<LineRenderer>().SetPositions(new Vector3[]
                {
                (nextStageList[i].locationPos - transform.localPosition) * 0.1f,
                (nextStageList[i].locationPos - transform.localPosition) * 0.9f
                });
            }
    }

    private void addPrevStage(StageController stage)
    {
        if (!prevStageList.Contains(stage))
        {
            prevStageList.Add(stage);
        }
    }

    private void removePrevStage(StageController stage)
    {
        if (prevStageList.Contains(stage))
        {
            prevStageList.Remove(stage);
        }
    }

    public void setStageType(StageType stageType)
    {
        this.stageType = stageType;
        bgSprite.color = colorPicker(stageType);
    }

    public StageType getStageType()
    {
        return stageType;
    }

    public List<StageController> getPrevStageControllers()
    {
        return prevStageList;
    }

    public int getIdx()
    {
        return idx;
    }

    public List<float> getLocationPos()
    {
        return new List<float>() { locationPos.x, locationPos.y };
    }

    public List<int> getNextStageIds()
    {
        if (nextStageList == null || nextStageList.Length == 0) return null;
        List<int> ids = new List<int>();
        foreach (StageController controller in nextStageList)
        {
            ids.Add(controller.idx);
        }
        return ids;
    }

    public void callNextStages(Dictionary<string, StageController> stageControllers)
    {
        if (nextIds == null || nextIds.Count == 0) return;
        nextStageList = new StageController[nextIds.Count];
        int cnt = 0;
        foreach (int nextId in nextIds)
        {
            nextStageList[cnt++] = stageControllers[nextId.ToString()];
            stageControllers[nextId.ToString()].addPrevStage(this);
        }
        drawLine();
    }
}
