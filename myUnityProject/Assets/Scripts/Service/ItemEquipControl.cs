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
        //
        //public MemberService memberService;
        //public CharacterControl characterControl;
        public CommonUtil commonUtil;

        public GameObject prefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        //public Sprite itemIconSprite;
        //public Sprite unitIconSprite;
        //public Text priceText;


        public SpriteAtlas AtlasItem;

        public UserCharEquipItemPacket userCharEquipItemPacket;
        public List<ItemView> views = new List<ItemView>();

        public ArrayList selectedEquipItemAry = new ArrayList();
        //public UserCharacter userCharacter;

        //private HttpSock httpSock;

        private void Start()
        {
            AtlasItem = Resources.Load<SpriteAtlas>("Atlas/ItemSpriteAtlas") as SpriteAtlas;

            //characterPacket = characterControl._CharacterPacket;
            //userCharacter = userSession._UserCharacter;
            //httpSock = userSession._HttpObject;
        }
        //선택한 아이템 정보를 배열에 저장 
        public void SelectMultiEquipItem(string item_uniqueID)
        {
            selectedEquipItemAry.Add(item_uniqueID);
            Debug.Log("item_uniqueID:" + item_uniqueID);
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
        public void EquipItem (int equip_yn, int item_id, int item_uniqueID, int item_category)
        {
            int job_code = (equip_yn == 1) ? 2 : 1; //1:장착, 2: 장착해제 

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
                    
                    //todo : change design style                                                                                                                         model.item_category));
                    if(model.equip_yn == 2) //장착해제 
                    {
                        ColorBlock colors = instance.GetComponentInChildren<Button>().colors;
                        colors.normalColor = Color.grey;
                        //colors.highlightedColor = new Color32(255, 100, 100, 255);
                        instance.GetComponentInChildren<Button>().colors = colors;
                    }
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

            //view.icon3Image.texture = availableIcons[model.icon3Index];

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

            public ItemView(Transform rootView)
            {
                itemId = rootView.Find("txt_itemId").GetComponent<Text>();
                itemImg = rootView.Find("img_item").GetComponent<Image>();
                // itemImg = rootView.Find("img_item").GetComponent<SpriteRenderer>();
            }
        }

    }
}