using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject loadingGO, curtainGO;
    [SerializeField]
    SettingManager settingManager;
    [SerializeField]
    private bool isForEditor = false;
    public static Vector2 screenSize = Vector2.zero;
    public static float screenUnitSize = 0f;
    public static float adjustFontSize = 8f;

    private bool isSettingOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        LocalStorage.EDITOR_MODE = isForEditor;
        if (LocalStorage.EDITOR_MODE)
        {
            LocalStorage.UIOpenStatus.fade = false;
        }
        getScreenSize();
        if (LocalStorage.UIOpenStatus.fade && !isForEditor)
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

    public void controllLoading(bool isFadeIn, string targetSceneName, bool isMainLoading = true)
    {
        StartCoroutine(fadeScreen(isFadeIn, targetSceneName, isMainLoading));
    }

    private IEnumerator fadeScreen(bool isFadeIn, string targetSceneName, bool isMainLoading = true)
    {
        GameObject targetCurtain = isMainLoading ? loadingGO : curtainGO;
        if (isFadeIn)
        {
            targetCurtain.SetActive(isFadeIn);
        }
        yield return new WaitForSeconds(0.2f);
        float cnt = 0f;
        while (cnt < 0.5f)
        {
            cnt += 0.01f;
            yield return new WaitForSeconds(0.01f);
            targetCurtain.GetComponent<Image>().color = new Color(0f, 0f, 0f, isFadeIn ? 2f * cnt : 2f * (0.5f - cnt));
        }
        yield return new WaitForSeconds(0.2f);
        if (targetSceneName != null)
        {
            SceneManager.LoadScene(targetSceneName);
            LocalStorage.UIOpenStatus.fade = true;
        }
        if (!isFadeIn)
        {
            targetCurtain.SetActive(isFadeIn);
            LocalStorage.UIOpenStatus.fade = false;
        }
    }

    public void saveGame()
    {
        SaveFunction.saveData();
    }

    public void saveAndReturnToMainMenu()
    {
        saveGame();
        loadScene("MainMenuScene");
    }

    public void saveAndExitGame()
    {
        saveGame();
        exitGame();
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
