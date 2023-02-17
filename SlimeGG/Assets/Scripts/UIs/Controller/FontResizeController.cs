using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontResizeController : ObjectSizeController
{
    [SerializeField]
    public float fonsSizeRatioToUnit = 1f;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        GetComponent<TextMeshProUGUI>().fontSize = MainGameManager.adjustFontSize * fonsSizeRatioToUnit;
    }
}
