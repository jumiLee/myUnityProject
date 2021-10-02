using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D;

namespace Service
{
    public class ItemControl : MonoBehaviour
    {
        public HttpSock httpSock;
        public MemberService memberService;
        public CharacterControl characterControl;
        public CommonUtil commonUtil;

        public GameObject itemPrefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        //public Sprite itemIconSprite;
        //public Sprite unitIconSprite;
        //public Text priceText;


        public SpriteAtlas AtlasItem;

        public ItemPacket itemPacket;
        public List<ItemView> views = new List<ItemView>();

        public ArrayList selectedItemAry = new ArrayList();

        private void Start()
        {
            AtlasItem = Resources.Load<SpriteAtlas>("Atlas/ItemSpriteAtlas") as SpriteAtlas;

        }
        //선택한 아이템 정보를 배열에 저장 
        public void SelectMultiItem(string item_id)
        {
            selectedItemAry.Add(item_id);
        }

        //카테고리별 아이템 조회 
        public void GetItemListByCategory(Text item_category)
        {

            string json = httpSock.Connect("selectItemLisByCategory.do",
                                           "user_account=" + memberService._MemberInfoPacket.account
                                         + "&item_category=" + item_category.text);

            itemPacket = JsonUtility.FromJson<ItemPacket>(json);

            //Generate item list
            if (itemPacket.resultCd == 0)
            {
                GenerateItemList(itemPacket.itemList);
            }
            else
            {
                commonUtil.HandleAlert(itemPacket.resultMsg);
            }
        }

        //아이템 구매 
        public void BuyAndEquipItem()
        {
            CharacterPacket characterPacket = characterControl._CharacterPacket;

            string json = httpSock.Connect("buyAndEquipItem.do",
                                           "user_account=" + characterPacket.account
                                         + "&char_id=" + characterPacket.carryUserCharacter.char_id
                                         + "&user_char_sn=" + characterPacket.carryUserCharacter.user_char_sn
                                         + "&item_id_array=" + string.Join(",", selectedItemAry.ToArray())
                                         );

            itemPacket = JsonUtility.FromJson<ItemPacket>(json);

            //Generate item list
            if (itemPacket.resultCd == 0)
            {
                GenerateItemList(itemPacket.itemList);
            }
            else
            {
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
            foreach(var model in itemList)
            {
                GameObject instance = GameObject.Instantiate(itemPrefab.gameObject) as GameObject;
                instance.transform.SetParent(content);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);

                //생성한 버튼에 onClick Event 생성 
                instance.GetComponent<Button>().onClick.AddListener(() => SelectMultiItem(model.item_id.ToString()));
                instance.GetComponentInChildren<Text>().text = model.item_id.ToString();
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