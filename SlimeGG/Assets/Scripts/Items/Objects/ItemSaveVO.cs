using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemSaveVO : ItemDictionaryVO
{
    public string itemName { get; set; }
    public int installed { get; set; }

    public void setDetailedInfo(ItemDictionaryVO item)
    {
        this.src = item.src;
        this.effect = item.effect;
    }
}