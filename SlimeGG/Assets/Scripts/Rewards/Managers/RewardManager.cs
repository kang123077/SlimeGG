using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    InfoWindowController infoWindowController;
    [SerializeField]
    Transform choicePrefab;

    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        trackCamera();
    }

    private void initSetting()
    {
        transform.localScale = new Vector3(18f, 10f, 1f);
    }

    private void trackCamera()
    {
        transform.position = new Vector3(
            Camera.main.transform.position.x,
            transform.position.y,
            1f);
    }

    private IEnumerator toggleCoroutine(bool isActive)
    {
        this.isActive = isActive;
        LocalStorage.IS_CAMERA_FREE = false;
        yield return new WaitForSeconds(3f);
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
        LocalStorage.IS_CAMERA_FREE = !isActive;
    }

    public void toggle(bool isActive, RewardType rewardType)
    {
        setRewardType(rewardType);
        StartCoroutine(toggleCoroutine(isActive));
    }

    public void setRewardType(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.Choice_Monster:
                GameObject listGO = new GameObject();
                listGO.transform.SetParent(transform);
                listGO.transform.localScale = Vector3.one;
                listGO.transform.localPosition = new Vector3(0f, 0f, 0f);

                // 보상 선택지 불러오기
                for (int i = 0; i < 3; i++)
                {
                    Transform newChoice = Instantiate(choicePrefab);
                    newChoice.SetParent(listGO.transform);
                    newChoice.localScale = new Vector3(
                        2f / 18f,
                        2f / 10f,
                        1f);
                    newChoice.localPosition = new Vector3(
                        (1f - i) * 0.25f,
                        2f / 10f,
                        0f
                        );
                    newChoice.GetComponent<ChoiceController>().setInfoWindowController(infoWindowController);
                }

                break;
        }
    }
}
