using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections;
using System.Collections.Generic;

namespace Service
{
    public class ItemControl : MonoBehaviour
    {
        public HttpSock httpSock;
        public MemberService memberService;
        public CommonUtil commonUtil;

        public GameObject itemPrefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        //public Sprite itemIconSprite;
        //public Sprite unitIconSprite;
        //public Text priceText;

        public ItemPacket itemPacket;
        public List<ItemView> views = new List<ItemView>();

        public ArrayList selectedItemAry = new ArrayList();

        //이정보를 담을 오브젝트를 별도로 만들어 관리해야 ...
        public void SelectMultiItem(string item_id)
        {
            selectedItemAry.Add(item_id);
            Debug.Log("선택한 아이템 개수 :"+selectedItemAry.Count);
        }

        public void GetItemListByCategory(Text item_category)
        //public void GetItemListByCategory(GameObject currentTabButton)
        {

            //string item_category = currentTabButton.GetComponent<Text>().text;

            Debug.Log("item_category:" + item_category.text);

            string json = httpSock.Connect("selectItemLisByCategory.do",
                                           "user_account=" + memberService._MemberInfoPacket.account
                                         + "&item_category=" + item_category.text);

            itemPacket = JsonUtility.FromJson<ItemPacket>(json);

            //Generate item list

            if (itemPacket.resultCd == 0)
            {
                Debug.Log("itemPacket:" + itemPacket.itemList.Count + "개의 아이템이 있습니다.");

                GenerateItemList(itemPacket.itemList);
            }
            else
            {
                //fail alert window 
                commonUtil.HandleAlert(itemPacket.resultMsg);
            }
        }

        //Generate item list with item prefab icon
        void GenerateItemList(List<Item> itemList)
        {

            //Destory pre-generated views
            foreach (Transform child in content) 
            {
                Destroy(child.gameObject);
            }
            views.Clear();

            //Generate new item list with itemList 
            int i = 0;
            foreach(var model in itemList)
            {
                GameObject instance = GameObject.Instantiate(itemPrefab.gameObject) as GameObject;
                instance.transform.SetParent(content);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);

                //생성한 버튼에 onClick Event 생성 
                instance.GetComponent<Button>().onClick.AddListener(() => SelectMultiItem(model.item_id.ToString()));
                instance.GetComponentInChildren<Text>().text = model.item_id.ToString();

                i++;

                views.Add(view);
            }
        }

        //make item view according to item class
        ItemView InitializeItemView(GameObject viewGameObject, Item model)
        {
            ItemView view = new ItemView(viewGameObject.transform);

            view.itemName.text = model.item_nm;
            view.itemPrice.text = model.item_price.ToString();
            view.itemId.text = model.item_id.ToString();
            //view.itemUnitImg = model.unit_cd;
            //view.itemImg = model.item_img_no.ToString();
            //view.icon3Image.texture = availableIcons[model.icon3Index];

            return view;
        }

        //ItemIconPrefab structure
        public class ItemView
        {
            public Text itemName;
            public Text itemPrice;
            public Text itemId;
            public Sprite itemUnit;
            public Sprite itemImg;


            public ItemView (Transform rootView)
            {
                itemPrice   = rootView.Find("txt_itemPrice").GetComponent<Text>();
                itemName = rootView.Find("txt_itemName").GetComponent<Text>();
                itemId = rootView.Find("txt_itemId").GetComponent<Text>();
                itemUnit = rootView.Find("img_unit").GetComponent<Sprite>();
                itemImg = rootView.Find("img_item").GetComponent<Sprite>();
            }
        }
    }
}