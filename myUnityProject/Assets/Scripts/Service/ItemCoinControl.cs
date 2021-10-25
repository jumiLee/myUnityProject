using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D;

namespace Service
{
    public class ItemCoinControl : MonoBehaviour
    {
        public UserSession userSession;
        public CommonUtil commonUtil;
        public InitialSetControl initialSetControl;

        public GameObject panel;

        public GameObject itemPrefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        public ItemPacket itemPacket;
        public ResultPacket resultPacket;
        public List<ItemView> views = new List<ItemView>();

        public Text user_gold;

        //카테고리별 아이템 조회 
        public void GetCoinItemList()
        {

            string json = userSession._HttpObject.Connect( "selectItemLisByCategory.do",
                                                           "user_account="   + userSession._UserDetail.account
                                                         + "&item_category=" + "90");

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

        //Item 구매 
        public void BuyItem(int item_id)
        {
            string json = userSession._HttpObject.Connect( "buyItem.do",
                                                           "user_account="   + userSession._UserDetail.account
                                                         + "&item_id_array=" + item_id.ToString());
            resultPacket = JsonUtility.FromJson<ResultPacket>(json);

            if (resultPacket.resultCd == 0)
            {
                commonUtil.HandleAlert("구매성공");
                initialSetControl.SetEssentialInfo();
                panel.SetActive(false);
            }
            else
            {
                commonUtil.HandleAlert(resultPacket.resultMsg);
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
            foreach (var model in itemList)
            {
                GameObject instance = GameObject.Instantiate(itemPrefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);

                //생성한 버튼에 onClick Event 생성 
                if (instance.GetComponentInChildren<Button>())
                {
                    instance.GetComponentInChildren<Button>()
                        .onClick.AddListener(() => BuyItem(model.item_id));
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

            view.item_price.text    = model.item_price.ToString();
            view.item_nm.text       = model.item_nm.ToString();
            view.item_desc.text     = model.item_desc.ToString();
            view.item_img.sprite    = Resources.Load<Sprite>("Images/item/" + model.item_img_no.ToString()) as Sprite;
            view.unit_cd_img.sprite = Resources.Load<Sprite>("Images/icon/unit_" + model.unit_cd.ToString()) as Sprite;

            return view;
        }

        //ItemIconPrefab structure
        [SerializeField]
        public class ItemView
        {
            public Text item_price;
            public Text item_nm;
            public Text item_desc;
            public Image item_img;
            public Image unit_cd_img;

            public ItemView(Transform rootView)
            {
                item_price  = rootView.Find("txt_item_price").GetComponent<Text>();
                item_nm     = rootView.Find("txt_item_nm").GetComponent<Text>();
                item_desc   = rootView.Find("txt_item_desc").GetComponent<Text>();
                item_img    = rootView.Find("img_item").GetComponent<Image>();
                unit_cd_img = rootView.Find("img_unit_cd").GetComponent<Image>();
            }
        }

    }
}