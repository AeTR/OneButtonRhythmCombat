using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundData : MonoBehaviour
{
    public RoundDataHolder[] roundDataHolders;
}

[System.Serializable]
public struct RoundDataHolder //gObj is the enemy you want to spawn. Beat is the amount of beats you want to pass AFTER the last enemy spawened before you spawn the enemy
{
    public int beat;
    public GameObject gObj;
}


