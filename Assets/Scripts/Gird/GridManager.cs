using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : Instancable<GridManager>
{
    [SerializeField] private int _gridX;
    [SerializeField] private int _gridY;
    [SerializeField] private float spacingX = 1f;
    [SerializeField] private float spacingY = 1f;
    
    [SerializeField] private Transform parent;
    
    public Gems[] gemTypes;
  
    private void Start()
    {
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {
                Vector3 spawnPos = new Vector3((parent.transform.localPosition.x)+(i*spacingX), 2, (parent.transform.localPosition.z)+(j*spacingY));
                Gems randomGem = GetRandomPrefab();
                GameObject newObject = Instantiate(randomGem.gemPrefab, spawnPos, Quaternion.identity,parent.transform);
                StartCoroutine(IncreaseSizeOverTime(newObject.transform));
            }
        }
    }
    private Gems GetRandomPrefab()
    {
        int randomIndex = Random.Range(0, gemTypes.Length);
        return gemTypes[randomIndex];
    }

    private IEnumerator IncreaseSizeOverTime(Transform targetTransform)
    {
        Vector3 initialScale = Vector3.zero;
        targetTransform.localScale = initialScale;

        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (targetTransform != null && targetTransform.localScale.x<=1.5f)
            {
                targetTransform.localScale += Vector3.one * .1f;
            }
        }
    }
    public void GemStack(Transform spawnPoints)
    {
        int randomIndex = Random.Range(0, gemTypes.Length);
        GameObject newGem= Instantiate(gemTypes[randomIndex].gemPrefab, spawnPoints.transform.position, Quaternion.identity,parent.transform);
        StartCoroutine(IncreaseSizeOverTime(newGem.transform));
    }
}
