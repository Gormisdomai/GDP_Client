﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Timers;
using UnityStandardAssets.CrossPlatformInput;

public class getData : MonoBehaviour
{

    //the name of the connection, not required but better for overview if you have more than 1 connections running
    public string conName = "GamingTheMarket";

    //ip/address of the server, 127.0.0.1 is for your own computer
    public string conHost = "tcp.ngrok.io";

    //port for the server, make sure to unblock this in your router firewall if you want to allow external connections
	public int conPort = 13168;

    //a true/false variable for connection status
    public bool socketReady = false;
    private bool connectionExpired = false;
    private const int TIMEOUT = 5000;

    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;

    //try to initiate connection
    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient(conHost, conPort);
            theStream = mySocket.GetStream();
            theStream.WriteTimeout = TIMEOUT;
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }

    //send message to server
    public void writeSocket(string theLine)
    {
        if (!socketReady)
            return;
        String tmpString = theLine + "\r\n";
        try
        {
            theWriter.Write(tmpString);
        }
        catch (Exception e)
        {
            Debug.Log("Connection expired:" + e);
            connectionExpired = true;
        }
        theWriter.Flush();
    }

    //read message from server
    public string readSocket()
    {
        String result = "";
        if (theStream.DataAvailable)
        {
            Byte[] inStream = new Byte[mySocket.SendBufferSize];
            theStream.Read(inStream, 0, inStream.Length);
            result += System.Text.Encoding.UTF8.GetString(inStream);
        }
        return result;
    }

    //disconnect from the socket
    public void closeSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }

    //keep connection alive, reconnect if connection lost
    public void maintainConnection()
    {
        if (!theStream.CanRead || connectionExpired)
        {
            Debug.Log("Connection reset");
            setupSocket();
            connectionExpired = false;
            resetTimer();
        }
    }

    //The timer marks the connection as expired unless
    //the client resets it before 5 seconds passed.
    private Timer aTimer;
    private void resetTimer()
    {
        if (aTimer != null) aTimer.Stop();
        aTimer = new System.Timers.Timer(TIMEOUT);

        aTimer.Elapsed += (sender, e) =>
        {
            Debug.Log("Connection expired");
            connectionExpired = true;
        };
        aTimer.AutoReset = false;
        aTimer.Enabled = true;
    }

    void Start()
    {
        setupSocket();
        resetTimer();
    }

    //We read from the socket once per frame.
    void Update()
    {
        maintainConnection();

        String resSocket = readSocket();
        if (resSocket != "") //if we have a new data point
        {
            resetTimer();
            //Debug.Log(resSocket); //print the response in console
			string[] allData = resSocket.Split('\n');
            foreach (String d in allData) {
                string[] data = d.Split(',');
				if (data.Length == 2) {
					GetComponent<SpikeSpawner> ().addSpike (new float[] { float.Parse (data [0]), float.Parse (data [1]) });
					if (!GameObject.Find ("Character").GetComponent<Death>().dead)
						Tick (PlayerPrefs.GetString ("name", "anon"), GameObject.Find ("Character").GetComponent<Rigidbody2D> ().velocity.y);
					}
				}
            }
    }

	public void Tick(String n, float v){
		print ("tick");
		String nickname = n.Replace (":", "").Replace ("{", "").Replace ("}", "").Replace ("\"", "");
		if (v != 0) {
			writeSocket ("move:"+ ((v > 0) ? 1 : -1).ToString ());
		}
		Score (nickname, GameObject.Find ("ScoreDisplay").GetComponent<ScoreUpdate> ().score);
	}

	public void Score(String nickname, float s){
		if (s != 0) {
			writeSocket ("score:"+nickname + ":" + s.ToString ());
		}
	}

}
