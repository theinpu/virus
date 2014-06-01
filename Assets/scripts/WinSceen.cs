using UnityEngine;
using System.Collections;

public class WinSceen : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/2f - 50f, Screen.height/2f + 60f, 100, 30), "New game"))
        {
            Application.LoadLevel("lobby");
        }
    }
}
