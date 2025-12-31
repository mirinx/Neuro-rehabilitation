using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class BCIReceiver : MonoBehaviour
{
    public int port = 5005;
    
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform head;

    private UdpClient udpClient;
    private Thread receiveThread;
    
    private int targetAction = -1;
    private float signalIntensity = 0f;
    private bool isRunning = true;

    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        udpClient = new UdpClient(port);
        while (isRunning)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                
                string[] parts = text.Split(',');
                if (parts.Length == 2)
                {
                    targetAction = int.Parse(parts[0]);
                    signalIntensity = float.Parse(parts[1], CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                break;
            }
        }
    }

    void Update()
    {
        float moveSpeed = 5f * signalIntensity; 
        if (signalIntensity < 0.1f) moveSpeed = 2f;

        if (targetAction == 0 && leftArm != null)
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, 90); 
            leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, targetRot, Time.deltaTime * moveSpeed);
        }
        else if (targetAction == 1 && rightArm != null)
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, -90);
            rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, targetRot, Time.deltaTime * moveSpeed);
        }
        else if (targetAction == 2 && leftFoot != null && rightFoot != null)
        {
            Quaternion targetRot = Quaternion.Euler(45, 0, 0);
            leftFoot.localRotation = Quaternion.Lerp(leftFoot.localRotation, targetRot, Time.deltaTime * moveSpeed);
            rightFoot.localRotation = Quaternion.Lerp(rightFoot.localRotation, targetRot, Time.deltaTime * moveSpeed);
        }
        else if (targetAction == 3 && head != null)
        {
             Quaternion targetRot = Quaternion.Euler(30, 0, 0);
             head.localRotation = Quaternion.Lerp(head.localRotation, targetRot, Time.deltaTime * moveSpeed);
        }
        else 
        {
            Quaternion resetRot = Quaternion.identity;
            
            if(leftArm) leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, resetRot, Time.deltaTime * 2f);
            if(rightArm) rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, resetRot, Time.deltaTime * 2f);
            if(head) head.localRotation = Quaternion.Lerp(head.localRotation, resetRot, Time.deltaTime * 2f);
            
            if(leftFoot) leftFoot.localRotation = Quaternion.Lerp(leftFoot.localRotation, resetRot, Time.deltaTime * 2f);
            if(rightFoot) rightFoot.localRotation = Quaternion.Lerp(rightFoot.localRotation, resetRot, Time.deltaTime * 2f);
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (receiveThread != null) receiveThread.Abort();
        if (udpClient != null) udpClient.Close();
    }
}