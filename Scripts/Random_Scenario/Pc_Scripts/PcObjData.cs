using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PcObjData", menuName = "ScriptableObjects/PcObjData", order = 2)]
public class PcObjData : ScriptableObject
{
    public List<GameObject> pcObjects;
    public List<int> pcNames;
}
