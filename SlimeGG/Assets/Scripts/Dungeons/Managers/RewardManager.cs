using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
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
    }

    public void toggle(bool isActive)
    {
        StartCoroutine(toggleCoroutine(isActive));
    }
}
