using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] float fadeTime=1f;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] List<Element> items = new List<Element>();
    [SerializeField] Scrollbar _VerticalLayoutGroup;
    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject itemPrefab;
    private void Start()
    {
        foreach (var item in items)
        {
            item.nameText.text = item.Name;
        }
    }

    private void DeleteChildren()
    {
        for (int i = contentTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }
    public void PanelFadeIn()
    {
        foreach (var inventoryElement in PlayerCollectGem.instance.SoldGems)
        {
            GameObject item=Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, contentTransform);
            Element element = item.GetComponent<Element>();
            element.nameText.text = inventoryElement.Key.ToString();
            element.gemCountText.text = inventoryElement.Value.ToString();
            element.icon.sprite = GridManager.instance.gemTypes.FirstOrDefault(g => g.gemName == inventoryElement.Key).gemIcon;
        }
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
        StartCoroutine(ItemsAnimation());
    }
    public void PanelFadeOut()
    {
        _VerticalLayoutGroup.value = 1f;
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = Vector3.zero;
        rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(0, fadeTime);
        DeleteChildren();
    }
    IEnumerator ItemsAnimation()
    {
        List<Element> itemsCopy = new List<Element>(items);
    
        foreach (var item in itemsCopy) 
        {
            item.prefab.transform.localScale = Vector3.zero;
        }

        foreach (var item in itemsCopy) 
        {
            item.prefab.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.25f);
        }
    }
    
    public void DeleteRecords()
    {
        PlayerPrefs.DeleteAll();
        PlayerCollectGem.instance.SoldGems.Clear();
        PlayerCollectGem.instance.totalGold = 0;
        PlayerCollectGem.instance.goldText.text = "0";
    }
}
