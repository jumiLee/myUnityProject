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

        public GameObject panel;

        public GameObject itemPrefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        public CashPacket cashPacket;
        public ResultPaymentPacket resultPaymentPacket;
        public ResultPacket resultPacket;
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

        //Cash 구매 
        public void BuyCash(int cash_id, int device_type, int payment_type)
        {
            //1. 결제 요청 
            string json = userSession._HttpObject.Connect( "requestPayment.do",
                                                           "user_account=" + userSession._UserCharacter.user_account
                                                         + "&cash_id=" + cash_id
                                                         + "&device_type=" + device_type
                                                         + "&payment_type=" + payment_type);
            resultPaymentPacket = JsonUtility.FromJson<ResultPaymentPacket>(json);

            if (resultPaymentPacket.resultCd == 0)
            {
                //TODO : 2. 결제사 통신 부분 추가 
                RequestPayment();

                //3.결제결과 업데이트 
                UpdatePayment(cash_id);

            }
            else
            {
                commonUtil.HandleAlert(resultPaymentPacket.resultMsg);
            }

        }

        //TODO : 결제사 통신 부분 (결제사에 맞게 수정 및 암호화 필요)
        private ResultPaymentPacket RequestPayment()
        {
            string orderNo = "testOrderNo";
            int trade_res_cd = 0;
            string trade_res_msg = "testResMsg";
            string trade_res_key = "ONLYGODWILLMAKEAWAY";

            resultPaymentPacket.orderNo = orderNo;
            resultPaymentPacket.trade_res_cd = trade_res_cd;
            resultPaymentPacket.trade_res_msg = trade_res_msg;
            resultPaymentPacket.trade_res_key = resultPaymentPacket.tradeNo+ trade_res_key;

            return resultPaymentPacket;
        }

        private void UpdatePayment(int cash_id)
        {
            string json = userSession._HttpObject.Connect("updatePayment.do",
                                                           "user_account="   + userSession._UserCharacter.user_account
                                                         + "&cash_id="       + cash_id
                                                         + "&trade_no="      + resultPaymentPacket.tradeNo
                                                         + "&order_no="      + resultPaymentPacket.orderNo
                                                         + "&trade_res_cd="  + resultPaymentPacket.trade_res_cd
                                                         + "&trade_res_msg=" + resultPaymentPacket.trade_res_msg
                                                         + "&trade_res_key=" + resultPaymentPacket.trade_res_key);

            resultPacket = JsonUtility.FromJson<ResultPacket>(json);
            if (resultPacket.resultCd == 0)
            {
                commonUtil.HandleAlert("구매성공");
                panel.SetActive(false);
            }
            else
            {
                commonUtil.HandleAlert(resultPacket.resultMsg);
            }
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
                    instance.GetComponentInChildren<Button>()
                        .onClick.AddListener(() => BuyCash(model.cash_id, model.device_type, model.payment_type));
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