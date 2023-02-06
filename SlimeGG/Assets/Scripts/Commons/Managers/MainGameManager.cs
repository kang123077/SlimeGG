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
    public void loadScene(string sceneToLoad)
    {
        controllLoading(true, sceneToLoad);
    }

    public void controllLoading(bool isFadeIn, string targetSceneName)
    {
        StartCoroutine(fadeScreen(isFadeIn, targetSceneName));
    }

    private IEnumerator fadeScreen(bool isFadeIn, string targetSceneName)
    {
        if (isFadeIn)
        {
            loadingGO.SetActive(isFadeIn);
        }
        float cnt = 0f;
        while (cnt < 1f)
        {
            cnt += 0.03f;
            yield return new WaitForSeconds(0.01f);
            loadingGO.GetComponent<Image>().color = new Color(0f, 0f, 0f, isFadeIn ? cnt : (1f - cnt));
        }
        if (targetSceneName != null)
        {
            SceneManager.LoadScene(targetSceneName);
            LocalStorage.IS_SCENE_FADE_IN = true;
        }
        if (!isFadeIn)
        {
            loadingGO.SetActive(isFadeIn);
            LocalStorage.IS_SCENE_FADE_IN = false;
        }
    }

    public void saveGame()
    {
        SaveFunction.saveData();
    }
}
