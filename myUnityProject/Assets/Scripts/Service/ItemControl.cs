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
        public UserSession userSession;
        public CommonUtil commonUtil;
        public InitialSetControl initialSetControl;

        public GameObject itemPrefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        public SpriteAtlas AtlasItem;
        public SpriteAtlas AtlasIcon;

        public ItemPacket itemPacket;
        public List<ItemView> views = new List<ItemView>();

        public ArrayList selectedItemAry = new ArrayList();

        private void Start()
        {
            //AtlasItem = Resources.Load<SpriteAtlas>("Atlas/ItemSpriteAtlas") as SpriteAtlas;
            //AtlasIcon = Resources.Load<SpriteAtlas>("Atlas/IconSpriteAtlas") as SpriteAtlas;
        }

        public void InitializeItem()
        {
            selectedItemAry = new ArrayList();
        }

        /*
        private void OnEnable()
        {

            Debug.Log("총 " + selectedItemAry.Count + " 개의 아이템 이미 선택");
            //Debug.Log("1.selectedItemAry.Count:" + selectedItemAry.Count);
            //selectedItemAry.Clear();
            selectedItemAry = new ArrayList();
            views.Clear();
            Debug.Log("2.selectedItemAry.Count:" + selectedItemAry.Count);
        }
        */
        //선택한 아이템 정보를 배열에 저장 
        public void SelectMultiItem(string item_id)
        {
            if(selectedItemAry.Contains(item_id))
            {
                commonUtil.HandleAlert("이미 선택한 아이템 입니다." );
            }
            else {
                selectedItemAry.Add(item_id);
            }
            //TODO :for debug
            Debug.Log("총 " + selectedItemAry.Count + " 개의 아이템 선택");
        }

        //카테고리별 아이템 조회 
        public void GetItemListByCategory(Text item_category)
        {

            string json = userSession._HttpObject.Connect("selectItemLisByCategory.do",
                                           "user_account=" + userSession._UserCharacter.user_account
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
            string json = userSession._HttpObject.Connect("buyAndEquipItem.do",
                                           "user_account=" + userSession._UserCharacter.user_account
                                         + "&char_id=" + userSession._UserCharacter.char_id
                                         + "&user_char_sn=" + userSession._UserCharacter.user_char_sn
                                         + "&item_id_array=" + string.Join(",", selectedItemAry.ToArray())
                                         );

            itemPacket = JsonUtility.FromJson<ItemPacket>(json);

            //아이템 구매 성공 시 
            if (itemPacket.resultCd == 0)
            {
                //메인정보 새로고침
                commonUtil.HandleAlert("구매성공");
                initialSetControl.SetEssentialInfo();

                //아이템 리스트 새로고침 
                GenerateItemList(itemPacket.itemList);
                InitializeItem();
            }
            else
            {
                commonUtil.HandleAlert(itemPacket.resultMsg);
                InitializeItem();
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
                instance.transform.SetParent(content, false);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);

                //생성한 버튼에 onClick Event 생성 
                if (instance.GetComponentInChildren<Button>())
                {
                    instance.GetComponentInChildren<Button>().onClick.AddListener(() => SelectMultiItem(model.item_id.ToString()));
                }
                else
                {
                    Debug.Log("object instantiate Failed!");
                }
                views.Add(view);
            }
        }

        //make item view according to item class
        ItemView InitializeItemView(GameObject viewGameObject, Item model)
        {
            ItemView view = new ItemView(viewGameObject.transform);

            view.itemName.text = model.item_nm;
            view.itemPrice.text = model.item_price.ToString();
            //view.itemId.text = model.item_id.ToString();
            //view.itemUnit.sprite = AtlasItem.GetSprite("unit"+model.unit_cd.ToString());
            view.itemUnit.sprite = Resources.Load<Sprite>("Images/icon/unit_" + model.unit_cd.ToString()) as Sprite;
            view.itemImg.sprite = Resources.Load<Sprite>("Images/item/" + model.item_img_no.ToString()) as Sprite;

            return view;
        }

        //ItemIconPrefab structure
        [SerializeField]
        public class ItemView
        { 
            public Text itemName;
            public Text itemPrice;
            public Text itemId;
            public Image itemUnit;
            public Image itemImg;

            public ItemView (Transform rootView)
            {
                itemPrice   = rootView.Find("txt_itemPrice").GetComponent<Text>();
                itemName = rootView.Find("txt_itemName").GetComponent<Text>();
                //itemId = rootView.Find("txt_itemId").GetComponent<Text>();
                itemUnit = rootView.Find("img_unit").GetComponent<Image>();
                itemImg = rootView.Find("img_item").GetComponent<Image>();
               // itemImg = rootView.Find("img_item").GetComponent<SpriteRenderer>();
            }
        }

    }
}