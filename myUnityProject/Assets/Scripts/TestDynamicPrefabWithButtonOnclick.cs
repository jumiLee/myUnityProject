using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDynamicPrefabWithButtonOnclick : MonoBehaviour
{

    public GameObject prefabSample;
    public Transform container;

    // Start is called before the first frame update
    //void Start()
    public void GenerateButton()
    {
       for (int i=0; i<5; i++)
        {
            GameObject go = Instantiate(prefabSample) as GameObject;
            go.transform.SetParent(container);
            Debug.Log(i);
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(i.ToString() + " button"));
            go.GetComponentInChildren<Text>().text = i.ToString() + " button";
        }
    }

    public void OnButtonClick(string imgName)
    {
        Debug.Log(imgName);
    }
}
