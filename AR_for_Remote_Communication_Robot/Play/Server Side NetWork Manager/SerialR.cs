using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UniRx;

public class SerialR : MonoBehaviour
{

    private static SerialPort sp = new SerialPort("COM8", 115200);
    bool isLoop = true;
    private Thread thread_;
    private string message_;
    Boolean NewMessag = false;
    // Use this for initialization
    void Start()
    {
        
        
            OpenConnection();
            
      
       

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("Send message F");
            sp.Write("f");
        }
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("Send message B");
            sp.Write("b");
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("Send message S");
            sp.Write("s");
        }
        if (Input.GetKey(KeyCode.Y))
        {
            Debug.Log("Send message Y");
            sp.Write("y");
        }
        if (Input.GetKey(KeyCode.N))
        {
            Debug.Log("Send message N");
            sp.Write("n");
        }
        if (NewMessag) {
            OnDataReceived(message_);

        }
        NewMessag = false;
        

    }
    void OnDestroy()
    {
        Debug.LogWarning("OnDestroy");
        Close();
    }

    private void Close()
    {
        isLoop = false;

        /*if (thread_ != null && thread_.IsAlive)
        {
            thread_.Join();
        }*/

        if (sp!= null && sp.IsOpen)
        {
            sp.Close();
            sp.Dispose();
        }
    }

    /* public void ReadData()
     {
         Debug.LogWarning("Read1");
         while (isLoop && serialPort_ != null && serialPort_.IsOpen)
         {
             try
             {
                 message_ = sp.ReadLine();
                 Debug.LogWarning(message_);
                 NewMessag = true;
             }
             catch (System.Exception e)
             {
                 Debug.LogWarning(e.Message);
             }
         }
     }*/




    public void Read()
    {
        Debug.LogWarning("Read1");
        while (isLoop && sp!= null && sp.IsOpen)
        {
            try
            {
                // if (serialPort_.BytesToRead > 0) {
                message_ = sp.ReadLine();
                Debug.Log(message_);
                NewMessag = true;
                // }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
                sp.Close();
                Debug.LogError("Failed to open Serial Port, already open!");
            }
            else
            {
                sp.Open();
                Scheduler.ThreadPool.Schedule(() => Read()).AddTo(this);
                sp.ReadTimeout = 50;
                Debug.Log("Open Serial port");
            }
        }
    }
    public void Write(string message)
    {
        try
        {
            sp.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
 
    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        if (data.Length < 2) return;

        try
        {
            Debug.Log(data);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}