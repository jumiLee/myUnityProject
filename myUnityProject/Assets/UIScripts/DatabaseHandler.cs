using UnityEngine;
using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using Mysql.Data;
//using Mysql.Data.MySqlClient;

public class DatabaseHandler : MonoBehaviour
{
    /*
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MysqlCommand cmd = null;
    private MysqlDataReader rdr = null;

    private MD5 _md5Hash;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        connectionString = "Server=" + host + ";" +
                            "Database=" + database + ";" +
                            "User=" + user + ";" +
                            "Password=" + password + ";" +
                            "Pooling=";
        if(pooling)
        {
            connectionString += "true;"; 
        }
        else
        {
            connectionString += "false;";
        }

        try
        {
            con = new MySqlConnection(connectionString);
            con.Open();
            Debug.Log("Mysql State:" + con.State);
        }
        catch (Exception e)
        {

        }
    }
    */
}
