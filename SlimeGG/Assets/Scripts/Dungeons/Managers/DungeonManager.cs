using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    StageController curStage;
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Transform userTf;

    private bool isFocusDone = false;

    int[] curPos = new int[] { 0, 0 };
    // Start is called before the first frame update
    void Start()
    {
        setSetting();
        setClear();
        setUser(curStage);
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.IS_SCENE_FADE_IN && !isFocusDone)
        {
            StartCoroutine(focusCamera(curStage));
        }
    }

    private IEnumerator focusCamera(StageController targetStage)
    {
        isFocusDone = true;
        LocalStorage.IS_CAMERA_FREE = false;
        while (Mathf.Abs(mainCamera.position.x - targetStage.transform.position.x) > 0.1f)
        {
            mainCamera.Translate(Vector3.left * (mainCamera.position.x - targetStage.transform.position.x) * Time.deltaTime * 2.5f);
            yield return new WaitForSeconds(0f);
        }
        LocalStorage.IS_CAMERA_FREE = true;
    }

    private void setSetting()
    {
        curStage.setDungeonManager(this);
        curStage.openAccessNext();
    }

    private void setClear()
    {
        curStage.clearStage();
    }

    private void setUser(StageController targetStage)
    {
        userTf.position = targetStage.transform.position;
    }

    public void moveCamera(StageController targetStage)
    {
        if (LocalStorage.IS_CAMERA_FREE)
        {
            StartCoroutine(focusCamera(targetStage));
        }
    }

    public void enterStage(StageController targetStage)
    {
        curStage.closeAccessNext();
        moveCamera(targetStage);
        curStage.clearStage();
        curStage = targetStage;
        setUser(curStage);
        curStage.openAccessNext();
    }
}
