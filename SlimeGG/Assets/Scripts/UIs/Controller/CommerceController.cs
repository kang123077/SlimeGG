using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommerceController : MonoBehaviour
{
    private TextMeshProUGUI currencyText;
    // Start is called before the first frame update
    void Start()
    {
        currencyText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        currencyText.text = $"";
    }

    // Update is called once per frame
    void Update()
    {
        if (currencyText != null)
            currencyText.text = $"{LocalStorage.Live.currency}";
    }
}
