using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown() {
        Destroy(transform.parent.gameObject);
        GameController.Instance.player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
