using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class PlayerCollectGem : Instancable<PlayerCollectGem>
{
    [SerializeField] private Transform stackParent;
    [SerializeField] private float stackOffset = 0.5f;
    [SerializeField] private float stackRotation = 30;
    
    private bool _isInsideArea = false; 
    private Coroutine _sellingCoroutine;
    
    
    public Stack<GameObject> GemStack = new Stack<GameObject>();
    public Dictionary<string, int> SoldGems = new Dictionary<string, int>();
    public Text goldText;
    public int totalGold;

    private void Start()
    {
        totalGold = PlayerPrefs.GetInt("TotalGold", 0);

        foreach (var type in GridManager.instance.gemTypes)
        {
           int a= PlayerPrefs.GetInt(type.gemName,0);

           if (a != 0)
           {
               SoldGems.Add(type.gemName,a);
           }
     
        }
        goldText.text = totalGold.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        Gems gem = other.transform.GetComponent<Gems>();
        if (gem != null && gem.transform.localScale.magnitude>=0.25f)
        {
            AddGameToStack(gem);
            Destroy(other.gameObject);
            GridManager.instance.GemStack(gem.transform);
        }
        if (other.CompareTag("SalesArea"))
        {
            _isInsideArea = true;
            _sellingCoroutine = StartCoroutine(SellGems());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SalesArea"))
        {
            _isInsideArea = false;
            if (_sellingCoroutine != null)
            {
                StopCoroutine(_sellingCoroutine);
            }
        }
    }
    private void AddGameToStack(Gems gem)
    {
        GameObject newGem = Instantiate(gem.gemPrefab, Vector3.zero, Quaternion.identity,stackParent);
        GemStack.Push(newGem);
        Vector3 stackPosition = Vector3.up * stackOffset * GemStack.Count;
        Quaternion stackRotation = Quaternion.Euler(0f, this.stackRotation * GemStack.Count, 0f);
        newGem.transform.localPosition = stackPosition;
        newGem.transform.localRotation = stackRotation;
    }
    
    private IEnumerator SellGems()
    {
        while (_isInsideArea && GemStack.Count > 0)
        {
            GameObject topGem = GemStack.Pop();

            if (topGem != null)
            {
                Gems gemComponent = topGem.GetComponent<Gems>();
                
                if (gemComponent != null)
                {
                    int gemValue = (int)(gemComponent.startingSalePrice + gemComponent.transform.localScale.x * 100);
                    totalGold += gemValue;
                    goldText.text = totalGold.ToString();
                    PlayerPrefs.SetInt("TotalGold", totalGold);

                    if (SoldGems.ContainsKey(gemComponent.gemName))
                    {
                        SoldGems[gemComponent.gemName]++;
                    }
                    else
                    {
                        SoldGems.Add(gemComponent.gemName, 1);
                    }

                    foreach (var soldGem in SoldGems)
                    {
                        PlayerPrefs.SetInt(soldGem.Key,soldGem.Value);
                    }
                    Destroy(topGem);
                }
                else
                {
                    yield break;
                }
            }
            else
            {
                yield break;
            }

            yield return new WaitForSeconds(1.5f);
        }
    }
    
}