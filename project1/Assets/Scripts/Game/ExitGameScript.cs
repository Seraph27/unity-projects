using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown() {
        doExitGame();
    }

    void doExitGame() {
        Debug.Log("EXITING");
        Application.Quit();
    }
}
