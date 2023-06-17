using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Gems:MonoBehaviour
{
    public string gemName;
    public float startingSalePrice;
    public Sprite gemIcon;
    public GameObject gemPrefab;
}
