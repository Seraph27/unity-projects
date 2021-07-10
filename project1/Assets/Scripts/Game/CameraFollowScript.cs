using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    GameObject player;
    public int depth = -10;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
    }

    // Update is called once per frame
    void Update()
    { 

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, depth);
       
    }
}
