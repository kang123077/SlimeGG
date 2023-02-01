using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenScreenManager : MonoBehaviour
{
    [SerializeField]
    string keyToToggle;
    bool isActive = false;
    bool isAnimating = false;
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        trackCamera();
        checkKeyPress();
    }

    private void initSetting()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition = new Vector3(0, Screen.height, 1f);
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    private void trackCamera()
    {
        transform.position = new Vector3(
            Camera.main.transform.position.x,
            transform.position.y,
            1f);
    }

    private void checkKeyPress()
    {
        if (Input.GetKeyDown(keyToToggle) && !isAnimating)
        {
            StartCoroutine(toggleCoroutine());
        }
    }

    private IEnumerator toggleCoroutine()
    {
        isActive = !isActive;
        LocalStorage.IS_CAMERA_FREE = false;
        isAnimating = true;
        while (isActive ? (transform.position.y > 0.01) : (transform.position.y < 9.99))
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate((isActive ? Vector3.down : Vector3.up) * (transform.position.y > 1 ? transform.position.y : 1f) * Time.deltaTime * 60);
        }
        transform.position = new Vector3(
            transform.position.x,
            isActive ? 0f : 10f,
            1f
            );
        isAnimating = false;
        LocalStorage.IS_CAMERA_FREE = !isActive;
    }
}
