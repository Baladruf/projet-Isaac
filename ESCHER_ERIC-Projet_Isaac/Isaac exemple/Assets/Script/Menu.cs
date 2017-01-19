using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public void onStart()
    {
        Application.LoadLevel("scene0");
    }

    public void onTest()
    {
        Application.LoadLevel("sceneCollect");
    }

    public void onQuit()
    {
        Application.Quit();
    }

    public void onMenu()
    {
        Application.LoadLevel("MENU");
    }
}
