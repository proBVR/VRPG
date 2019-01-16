using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class UDPMove : MonoBehaviour {
    //通信レート10/s

    const int LOCAL_PORT = 22221;
    static UdpClient udp;
    Thread thread;

    public static Action<float, float, float> AccelCallBack;

    public void UDPStart()
    {
        udp = new UdpClient(LOCAL_PORT);
        udp.Client.ReceiveTimeout = 3000;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    private static void ThreadMethod()
    {
        while (true)
        {
            byte[] data;
            try
            {
                IPEndPoint remoteEp = null;
                data = udp.Receive(ref remoteEp);

                string text = Encoding.ASCII.GetString(data);

                JsonNode jsonNode = JsonNode.Parse(text);

                double ax = jsonNode["sensordata"]["accel"]["x"].Get<double>();
                double ay = jsonNode["sensordata"]["accel"]["y"].Get<double>();
                double az = jsonNode["sensordata"]["accel"]["z"].Get<double>();

                AccelCallBack((float)ax, (float)ay, (float)az);
            }
            catch (SocketException se)
            {
                udp.Close();
                udp = new UdpClient(LOCAL_PORT);
                udp.Client.ReceiveTimeout = 3000;
                Debug.Log(se);
            }
            catch (NullReferenceException nre)
            {
                Debug.Log(nre);
            }
            catch (InvalidCastException ice)
            {
                Debug.Log(ice);
            }
        }
    }

    void OnApplicationQuit()
    {
        thread.Abort();
        udp.Close();
    }
}
