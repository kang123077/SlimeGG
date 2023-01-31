using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    StageController curStage;
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Transform userTf;
    MainGameManager mainGameManager;

    private bool isFocusDone = false;

    // Start is called before the first frame update
    void Start()
    {
        setSetting();
        setClear();
        applyJourney();
        setUser(curStage);
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.IS_SCENE_FADE_IN && !isFocusDone)
        {
            StartCoroutine(focusCamera(true, curStage));
        }
    }

    private IEnumerator focusCamera(bool isInit, StageController targetStage)
    {
        isFocusDone = true;
        LocalStorage.IS_CAMERA_FREE = false;
        while (Mathf.Abs(mainCamera.position.x - targetStage.transform.position.x) > 0.1f)
        {
            mainCamera.Translate(Vector3.left * (mainCamera.position.x - targetStage.transform.position.x) * Time.deltaTime * 2.5f);
            yield return new WaitForSeconds(0f);
        }
        if (isInit)
        {
            LocalStorage.IS_CAMERA_FREE = true;
        } else
        {
            mainGameManager.controllLoading(true, "BattleScene");
        }
    }

    private void applyJourney()
    {
        foreach (int stagePos in LocalStorage.journeyInfo)
        {
            curStage.clearStage();
            enterStage(curStage.getNextStage(stagePos));
        }
    }

    private void setSetting()
    {
        mainGameManager = transform.GetComponent<MainGameManager>();
        curStage.setDungeonManager(this);
    }

    private void setClear()
    {
        curStage.clearStage();
        curStage.openAccessNext();
    }

    private void setUser(StageController targetStage)
    {
        userTf.position = targetStage.transform.position;
    }

    public void moveCamera(StageController targetStage)
    {
        if (LocalStorage.IS_CAMERA_FREE)
        {
            StartCoroutine(focusCamera(false, targetStage));
        }
    }

    public void enterStage(StageController targetStage)
    {
        curStage.closeAccessNext();
        moveCamera(targetStage);
        //curStage.clearStage();
        curStage = targetStage;
        setUser(curStage);
        curStage.openAccessNext();
    }
}
