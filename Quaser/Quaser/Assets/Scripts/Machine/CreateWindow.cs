using AxiOMADataTest;
using System.Threading;
using UnityEngine;

public class CreateWindow : MonoBehaviour
{
    private Thread thread;

    public static float x;
    public static float y;
    public static float z;

    public static bool isRun;

    void Start()
    {
       thread = new Thread(() => Init());
       thread.Start();
    }

    private void Update()
    {
        if (Form1.isRun)
        {
            x = (float)Form1.x / 1000;
            y = -0.610f - ((float)Form1.z / 1000 * -1);
            z = (float)Form1.y / 1000;

            if (!isRun)
            {
                isRun = true;
            }
        }
    }

    private void Init()
    {
        Program.CreateWindow();
    }

    private void OnApplicationQuit() => thread.Abort();
}
