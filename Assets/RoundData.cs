using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundData : MonoBehaviour
{
    public RoundDataHolder[] roundDataHolders;
}

[System.Serializable]
public struct RoundDataHolder
{
    public int beat;
    public GameObject gObj;
}


