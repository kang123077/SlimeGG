using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontResizeController : MonoBehaviour
{
    [SerializeField]
    public float fonsSizeRatioToUnit = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().fontSize = MainGameManager.adjustFontSize * fonsSizeRatioToUnit;
    }
}
