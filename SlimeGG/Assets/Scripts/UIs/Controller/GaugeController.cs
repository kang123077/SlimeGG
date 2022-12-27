using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeController : MonoBehaviour
{
    private float maxWidth = 8f, maxHeight = 1f;
    private Transform backgroundTf;
    private Transform currentGaugeTf;
    private int maxGauge = 10, curGauge = 10;
    private float sideMargin = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initData(int maxGauge, float maxWidth, float maxHeight)
    {
        this.maxGauge = maxGauge;
        this.curGauge = maxGauge;
        this.maxWidth = maxWidth;
        this.maxHeight = maxHeight;
        backgroundTf = transform.Find("Background");
        currentGaugeTf = transform.Find("Current Gauge");
        updateSize();
    }

    public void addOrSubGauge(int amount)
    {
        curGauge = curGauge - amount > 0 ? curGauge - amount : 0;
        updateGauge();
    }

    private void updateSize()
    {

        backgroundTf.GetComponent<SpriteRenderer>().size = new Vector2(maxWidth, maxHeight);
        currentGaugeTf.GetComponent<SpriteRenderer>().size = new Vector2(maxWidth - sideMargin, maxHeight - sideMargin);
        updateGauge();
    }

    private void updateGauge()
    {
        float newWidth = maxWidth * curGauge / maxGauge;
        currentGaugeTf.GetComponent<SpriteRenderer>().size = new Vector2(newWidth - sideMargin, maxHeight - sideMargin);
        currentGaugeTf.GetComponent<RectTransform>().localPosition = new Vector3(-(maxWidth - newWidth) / 2, 0f, 0f);
    }
}
