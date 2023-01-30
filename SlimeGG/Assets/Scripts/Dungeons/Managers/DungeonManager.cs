using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    StageController initStage;
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Transform userTf;

    private bool isFocusDone = false;

    int[] curPos = new int[] { 0, 0 };
    // Start is called before the first frame update
    void Start()
    {
        setUser();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.IS_SCENE_FADE_IN && !isFocusDone)
        {
            StartCoroutine(focusCamera());
        }
    }

    private IEnumerator focusCamera()
    {
        isFocusDone = true;
        while (Mathf.Abs(mainCamera.position.x - initStage.transform.position.x) > 0.1f)
        {
            mainCamera.Translate(Vector3.left * (mainCamera.position.x - initStage.transform.position.x) * Time.deltaTime * 2.5f);
            yield return new WaitForSeconds(0f);
        }
    }

    private void setUser()
    {
        userTf.position = initStage.transform.position;
    }
}
