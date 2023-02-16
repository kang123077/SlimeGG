using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventController : ObjectSizeController
{
    private Image imageBackground, imageLeft, imageRight;
    private TextMeshProUGUI textDesc;
    private int curStatus = 0;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        switch (curStatus)
        {
            case 0:
                // 객체 연결
                initSetting();
                break;
            case 1:
                // 
                break;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
    }

    private void initSetting()
    {
        imageBackground = transform.GetChild(0).GetComponent<Image>();
        imageLeft = transform.GetChild(1).GetComponent<Image>();
        imageRight = transform.GetChild(2).GetComponent<Image>();
        textDesc = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        curStatus = 1;
    }
}
