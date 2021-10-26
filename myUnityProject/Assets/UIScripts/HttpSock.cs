
using UnityEngine;
using System.Net;
using System.Text;
using System.IO;
using System;

using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class HttpSock : MonoBehaviour
{  
    private static Mutex mut = new Mutex();
    string MainURL = "http://ec2-15-165-19-192.ap-northeast-2.compute.amazonaws.com:8080/mvWeb/"; 
    //string MainURL = "http://localhost:8080/mvWeb/"; //jumi-macbook
    public string tempRequestURL = "";
    public string tempRequestParam = "";
   
    public string getMainURL()
    {
        return this.MainURL;
    }
    public string Connect(string URL, String sdString)
    {   
        mut.WaitOne();
       
        string res = MetConnect(MainURL+URL, sdString); 

        mut.ReleaseMutex();
        Debug.Log(res);
        return res;
    }
    public string MetConnect(string URL, String sdString)
    {


        try
        {
            // 인코딩 1 - UTF-8

             
            Debug.Log("URL " + URL + "?" + sdString);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(URL);
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.Method = "POST";
            //myRequest.Timeout = 5000;//5초 타임아웃
            myRequest.Timeout = 20000;//20초 타임아웃

            byte[] sendData = UTF8Encoding.UTF8.GetBytes(sdString);

            myRequest.ContentLength = sendData.Length;
            Stream requestStream = myRequest.GetRequestStream();
            requestStream.Write(sendData, 0, sendData.Length);
            requestStream.Close(); 

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse(); 


            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close(); 
            result = result.Trim();
             


             return result;
        }
        catch (System.Net.WebException SCE)
        {
         
            Debug.Log("접속지연 : " + SCE.ToString()); 
            tempRequestURL = URL;
            tempRequestParam = sdString;
            // return "";
        }
        return null;
    }
 

   
    
    void Start()//tempRequestURL+"attendList.do"
    {
         
        DontDestroyOnLoad(gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
