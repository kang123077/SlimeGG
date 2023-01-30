using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject loadingGO;

    // Start is called before the first frame update
    void Start()
    {
        if (LocalStorage.IS_SCENE_FADE_IN)
        {
            controllLoading(false, null);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void controllLoading(bool isStart, string targetSceneName)
    {
        StartCoroutine(fadeScreen(isStart, targetSceneName));
    }

    public void loadScene(string sceneToLoad)
    {
        controllLoading(true, sceneToLoad);
    }

    public void endGame()
    {
        controllLoading(false, null);
    }

    private IEnumerator fadeScreen(bool isStart, string targetSceneName)
    {
        if (isStart)
        {
            loadingGO.SetActive(isStart);
        }
        float cnt = 0f;
        while (cnt < 1f)
        {
            cnt += 0.01f;
            yield return new WaitForSeconds(0.01f);
            loadingGO.GetComponent<Image>().color = new Color(0f, 0f, 0f, isStart ? cnt : (1f - cnt));
        }
        if (targetSceneName != null)
        {
            SceneManager.LoadScene(targetSceneName);
            LocalStorage.IS_SCENE_FADE_IN = true;
        }
        if (!isStart)
        {
            loadingGO.SetActive(isStart);
            LocalStorage.IS_SCENE_FADE_IN = false;
        }
    }
}
