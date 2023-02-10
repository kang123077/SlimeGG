using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject loadingGO;
    [SerializeField]
    SettingManager settingManager;
    public static Vector2 screenSize = Vector2.zero;
    public static float screenUnitSize = 0f;
    public static float adjustFontSize = 8f;

    private bool isSettingOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        getScreenSize();
        if (LocalStorage.UIOpenStatus.fade)
        {
            controllLoading(false, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        getScreenSize();
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
            LocalStorage.UIOpenStatus.fade = true;
        }
        if (!isFadeIn)
        {
            loadingGO.SetActive(isFadeIn);
            LocalStorage.UIOpenStatus.fade = false;
        }
    }

    public void saveGame()
    {
        SaveFunction.saveData();
    }
    public void toggleSetting()
    {
        isSettingOpen = !isSettingOpen;
        controllLoading(isSettingOpen, null);
        settingManager.toggle();
    }

    void getScreenSize()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        screenUnitSize = screenSize.y / 9f;
        adjustFontSize =
            screenUnitSize < 40f ? 8f : (screenUnitSize / 40f * 8f);
    }
}
