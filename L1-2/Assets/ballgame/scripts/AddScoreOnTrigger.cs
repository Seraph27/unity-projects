using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreOnTrigger : MonoBehaviour
{
    // public GameObject text;
    int score = 0;
    public TextMesh scoreText; 

    // Start is called before the first frame update
    void Start()
    {
    //    scoreText = text.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider c)
    {
        print(c.gameObject.name);
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
}
