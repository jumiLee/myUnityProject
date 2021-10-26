using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D;

namespace Service
{
    public class ItemEquipControl : MonoBehaviour
    {
        public UserSession userSession;
        public CommonUtil commonUtil;
        public InitialSetControl initialSetControl;

        public GameObject prefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        //public Sprite itemIconSprite;
        //public Sprite unitIconSprite;
        //public Text priceText;

        public SpriteAtlas AtlasItem;

        public UserCharEquipItemPacket userCharEquipItemPacket;
        public List<ItemView> views = new List<ItemView>();

        public ArrayList selectedEquipItemAry = new ArrayList();

        private void Start()
        {
           // AtlasItem = Resources.Load<SpriteAtlas>("Atlas/ItemSpriteAtlas") as SpriteAtlas;
        }
        //선택한 아이템 정보를 배열에 저장 
        public void SelectMultiEquipItem(string item_uniqueID)
        {
            selectedEquipItemAry.Add(item_uniqueID);
        }

        //Search My Inventory Items by Item Category 
        public void GetMyItemWithEquip(Text item_category)
        {
            string json = userSession._HttpObject.Connect("getMyItemWithEquip.do",
                                           "user_account="  + userSession._UserCharacter.user_account
                                         + "&char_id="      + userSession._UserCharacter.char_id
                                         + "&user_char_sn=" + userSession._UserCharacter.user_char_sn
                                         + "&item_category="+ item_category.text
                                         + "&item_type=0"   //All category
                                         );
             
            userCharEquipItemPacket = JsonUtility.FromJson<UserCharEquipItemPacket>(json);

            initialSetControl.SetEssentialInfo(); //inventory new icon update

            //Generate item list
            if (userCharEquipItemPacket.resultCd == 0)
            {   
                GenerateItemList(userCharEquipItemPacket.userCharEquipItemList);
            }
            else
            {
                commonUtil.HandleAlert(userCharEquipItemPacket.resultMsg);
            }
        }

        //Euqip Item
        public void EquipItem (string equip_yn, int item_id, int item_uniqueID, int item_category)
        {
            Debug.Log("equipItem call");

            int job_code = (equip_yn.Equals("Y")) ? 2 : 1; //1:장착, 2: 장착해제 

            string json = userSession._HttpObject.Connect("equipItem.do",
                                                           "job_code=" + job_code
                                                         + "&user_account=" + userSession._UserCharacter.user_account
                                                         + "&char_id=" + userSession._UserCharacter.char_id
                                                         + "&user_char_sn=" + userSession._UserCharacter.user_char_sn
                                                         + "&item_id=" + item_id
                                                         + "&item_uniqueID=" + item_uniqueID
                                                         + "&item_category=" + item_category
                                                         + "&item_type=0" 
                                                         );

            userCharEquipItemPacket = JsonUtility.FromJson<UserCharEquipItemPacket>(json);

            //Generate item list
            if (userCharEquipItemPacket.resultCd == 0)
            {
                initialSetControl.SetEssentialInfo(); //update character equip item info
                GenerateItemList(userCharEquipItemPacket.userCharEquipItemList);
            }
            else
            {
                commonUtil.HandleAlert(userCharEquipItemPacket.resultMsg);
            }
        }

        //Generate item list with item prefab icon
        void GenerateItemList(List<UserCharEquipItem> itemList)
        {
            //Destory pre-generated views
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            views.Clear();

            //Generate new item list with itemList 
            foreach (var model in itemList)
            {
                GameObject instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);

                //생성한 버튼에 onClick Event 생성 
                if(instance.GetComponentInChildren<Button>()) {
                    instance.GetComponentInChildren<Button>().onClick.AddListener(() => EquipItem(model.equip_yn,
                                                                                        model.item_id,
                                                                                        model.item_uniqueID,
                                                                                        model.item_category));
                }
                else
                {
                    Debug.Log("object instantiate Failed!");
                }
                instance.GetComponentInChildren<Text>().text = model.item_id.ToString();
                views.Add(view);
            }
        }

        //make item view according to item class
        ItemView InitializeItemView(GameObject viewGameObject, UserCharEquipItem model)
        {
            ItemView view = new ItemView(viewGameObject.transform);

            view.itemId.text = model.item_id.ToString();
            view.itemImg.sprite = Resources.Load<Sprite>("Images/item/" + model.item_img_no.ToString()) as Sprite;

            if (model.equip_yn.Equals("Y"))
            {
                view.selected_edge.enabled = true;
            }
            else
            {
                view.selected_edge.enabled = false;
            }
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
            public Image selected_edge;
            public Button btn_basic;

            public ItemView(Transform rootView)
            {
                itemId       = rootView.Find("txt_itemId").GetComponent<Text>();
                itemImg      = rootView.Find("img_item").GetComponent<Image>();
                selected_edge= rootView.Find("img_selected_edge").GetComponent<Image>();
                btn_basic    = rootView.Find("btn_bg_basic").GetComponent<Button>();

                // itemImg = rootView.Find("img_item").GetComponent<SpriteRenderer>();
            }
        }

    }
}