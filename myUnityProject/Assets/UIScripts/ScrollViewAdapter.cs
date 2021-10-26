
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class ScrollViewAdapter : MonoBehaviour
{
    public Texture2D[] availableIcons;
    public RectTransform prefab;
    public Text countText;
    public ScrollRect scrollView;
    public RectTransform content;

    List<ItemView> views = new List<ItemView>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateItems()
    {
        int newCount ;
        int.TryParse(countText.text, out newCount);
        //StartCoroutine(FetchItemModelsFromServer(newCount, results => OnReceivdNewModels(results)));
        OnReceivdNewModels(getModel());
    }

    //Get DB data
    ItemModel[] getModel()
    {
        var results = new ItemModel[10];

        for (int i = 0; i < 10; i++)
        {
            results[i] = new ItemModel();

            results[i].item_name = "Item Name" + i;
            results[i].item_price = "2,000(" + i + ")";
            results[i].icon1Index = UnityEngine.Random.Range(0, availableIcons.Length);
            results[i].icon1Index = UnityEngine.Random.Range(0, availableIcons.Length);
            //results[i].icon2Index = UnityEngine.Random.Range(0, availableIcons.Length);
            //results[i].icon3Index = UnityEngine.Random.Range(0, availableIcons.Length);
        }

        return results;

    }

    void OnReceivdNewModels(ItemModel[] models)
    {
        foreach(Transform child in content)
            Destroy(child.gameObject);
        views.Clear();

        int i = 0;
        foreach(var model in models)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);

              var view = InitializeItemView(instance, model);
            views.Add(view);

            ++i;
        }
    }

    ItemView InitializeItemView(GameObject viewGameObject, ItemModel model)
    {
        ItemView view = new ItemView(viewGameObject.transform);

        view.itemName.text = model.item_name;
        view.itemPrice.text = model.item_price;
        view.icon1Image.texture = availableIcons[model.icon1Index];
        view.icon2Image.texture = availableIcons[model.icon2Index];
        //view.icon3Image.texture = availableIcons[model.icon3Index];
         
        return view;
    }

    IEnumerator FetchItemModelsFromServer(int count, Action<ItemModel[]> onDone)
    {
        //Simulating Server delay
        yield return new WaitForSeconds(2f);

        var results = new ItemModel[count];

        for(int i=0; i< count; i++)
        {
            results[i] = new ItemModel();
            results[i].item_price = "Item Price" + i;
            results[i].item_name = "Item Name " + i;
            results[i].icon1Index = UnityEngine.Random.Range(0, availableIcons.Length);
            results[i].icon2Index = UnityEngine.Random.Range(0, availableIcons.Length);
            //results[i].icon3Index = UnityEngine.Random.Range(0, availableIcons.Length);
        }
        onDone(results);
    }

    public class ItemView
    {
        //public Text titleText;
        public Text itemPrice, itemName;
        public RawImage icon1Image, icon2Image, icon3Image;

        public ItemView(Transform rootView)
        {
            itemName = rootView.Find("Panel_item/txt_itemName").GetComponent<Text>();
            itemPrice  = rootView.Find("Panel_item/txt_itemName").GetComponent<Text>();

            icon1Image = rootView.Find("Panel_item/img_itemIcon").GetComponent<RawImage>();
            icon2Image = rootView.Find("Panel_item/img_unit").GetComponent<RawImage>();
            //icon1Image = rootView.Find("Icon1Image").GetComponent<RawImage>();
            //icon2Image = rootView.Find("Icon2Image").GetComponent<RawImage>();
            //icon3Image = rootView.Find("TitlePanel/Icon3Image").GetComponent<RawImage>();
        }
    }
    public class ItemModel
    {
        public string item_name;
        public string item_price;

        public int icon1Index, icon2Index;
        //public int icon1Index, icon2Index, icon3Index;

    }
}
