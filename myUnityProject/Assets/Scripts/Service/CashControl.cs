using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D;

namespace Service
{
    public class CashControl : MonoBehaviour
    {
        public UserSession userSession;
        public CommonUtil commonUtil;

        public GameObject itemPrefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        public CashPacket cashPacket;
        public List<ItemView> views = new List<ItemView>();

        public int device_type;
        public int payment_type;

        //카테고리별 아이템 조회 
        public void GetCashList()
        {

            string json = userSession._HttpObject.Connect("selectCashList.do",
                                           "user_account="  + userSession._UserDetail.account
                                         + "&device_type="  + device_type
                                         + "&payment_type=" + payment_type);

            cashPacket = JsonUtility.FromJson<CashPacket>(json);

            //Generate item list
            if (cashPacket.resultCd == 0)
            {
                GenerateItemList(cashPacket.cashList);
            }
            else
            {
                commonUtil.HandleAlert(cashPacket.resultMsg);
            }
        }

        //아이템 구매 
        public void BuyCash(int cash_id)
        {
            string json = userSession._HttpObject.Connect("buyAndEquipItem.do",
                                           "user_account=" + userSession._UserCharacter.user_account
                                         + "&char_id=" + userSession._UserCharacter.char_id
                                         + "&user_char_sn=" + userSession._UserCharacter.user_char_sn
                                         );
            commonUtil.HandleAlert("준비중입니다.");
            /*
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
            */
        }


        //Generate item list with item prefab icon
        void GenerateItemList(List<Cash> itemList)
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
                    instance.GetComponentInChildren<Button>().onClick.AddListener(() => BuyCash(model.cash_id));
                }
                else
                {
                    Debug.Log("object instantiate Failed!");
                }
                views.Add(view);
            }
        }

        //make item view according to item class
        ItemView InitializeItemView(GameObject viewGameObject, Cash model)
        {
            ItemView view = new ItemView(viewGameObject.transform);

            view.cash_amt.text      = model.cash_amt.ToString();
            view.cash_dis_amt.text  = model.cash_dis_amt.ToString();
            view.coin_nm.text       = model.coin_nm.ToString();
            view.coin_desc.text     = model.coin_desc.ToString();
            view.prod_img.sprite    = Resources.Load<Sprite>("Images/cash/prod_" + model.prod_img.ToString()) as Sprite;

            return view;
        }

        //ItemIconPrefab structure
        [SerializeField]
        public class ItemView
        {
            public Text cash_amt;
            public Text cash_dis_amt;
            public Text coin_nm;
            public Text coin_desc;
            public Image prod_img;

            public ItemView(Transform rootView)
            {
                cash_amt     = rootView.Find("txt_cash_amt").GetComponent<Text>();
                cash_dis_amt = rootView.Find("txt_cash_dis_amt").GetComponent<Text>();
                coin_nm      = rootView.Find("txt_coin_nm").GetComponent<Text>();
                coin_desc    = rootView.Find("txt_coin_desc").GetComponent<Text>();
                prod_img     = rootView.Find("img_prod").GetComponent<Image>();
            }
        }

    }
}