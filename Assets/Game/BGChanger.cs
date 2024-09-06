using Shop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGChanger : MonoBehaviour
{
    [SerializeField] private Image[] _bgImages;
    [SerializeField] private ItemSkeensShop _shop;
    [SerializeField] private SkeensData _data;

    private void Awake()
    {
        _shop.OnItemSelected += ChangeBG;
    }
    private void Start()
    {
        ChangeBG("BG", Saver.GetString("BG"));
    }
    public void ChangeBG(string itemType, string itemName)
    {
        if(itemType == "BG")
        {
            Sprite sprite = _data.GetSkeen(itemName);
            for (int i = 0; i < _bgImages.Length; i++)
            {
                _bgImages[i].sprite = sprite;
            }
        }



    }
}
