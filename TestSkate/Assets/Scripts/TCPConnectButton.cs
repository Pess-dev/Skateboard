using UnityEngine;
using TMPro;

public class TCPConnectButton : MonoBehaviour
{
    public TMP_InputField iPText;

    /// <summary>
    /// Connects to the server via TCPListener class with given IP (requaired) and port (optional) of iPText input field 
    /// </summary>
    public void Connect(){
        string[] splitted = iPText.text.Split(':');
        string ip = splitted[0];
        int port = 5045;

        if (splitted.Length > 1){
            port = int.Parse("0"+splitted[1]);
        }

        TCPListner.ConnectToServer(ip, port);        
    }
}
