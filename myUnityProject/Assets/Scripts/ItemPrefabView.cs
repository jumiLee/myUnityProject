using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entity;

[SerializeField]
public class ItemPrefabView 
{
    public Text itemName;
    public Text itemPrice;
    public Text itemId;
    public Image itemUnit;
    public Image itemImg;

    public ItemPrefabView(Item rootView)
    {
        itemPrice.text = rootView.item_price.ToString();
        itemName.text = rootView.item_nm;
        itemId.text = rootView.item_id.ToString();
        //itemUnit.sprite = rootView.Find("img_unit").GetComponent<SpriteRenderer>();
        //itemImg = rootView.Find("img_item").GetComponent<Sprite>();
        // itemImg = rootView.Find("img_item").GetComponent<SpriteRenderer>();
    }
}
