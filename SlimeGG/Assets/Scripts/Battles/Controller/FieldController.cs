using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField]
    public GridCellGenerator allyEntryGenerator, enemyEntryGenerator;
    public FieldInfo fieldInfo = null;
    private Transform monsterContainer = null;
}
