using Assets.MultiRougeLike.HexMap.Scripts;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI cityName;
    [SerializeField]
    GameObject ItemPrefab;
    [SerializeField]
    GameObject QuestPrefab;
    [SerializeField]
    Transform _itemContainer;
    [SerializeField]
    Transform _questContainer;
    public void FillData(City city)
    {
        foreach (var item in city.shop.Items)
        {
            var go = Instantiate(ItemPrefab, _itemContainer);
            var itemS = go.GetComponent<ItemScript>();
            itemS.Fill(item);
        }
    }
}
