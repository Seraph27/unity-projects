using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    float timer = 0.0f;
    Rigidbody2D rb;
    SpriteRenderer renderer;
    CircleCollider2D collider;
    public Sprite[] explosionAnimation;
    int explosionFrameCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
        collider.radius = 0.5f;
        StartCoroutine(BombCoroutine());
        
    }

    IEnumerator BombCoroutine() {
        rb.velocity = new Vector2(0, -2);
        yield return new WaitForSeconds(2);

        rb.velocity = Vector2.zero;
        renderer.color = Color.red;
        yield return new WaitForSeconds(2);

        renderer.color = Color.white;
        transform.localScale = new Vector3(3,3,1);
        collider.enabled = true;
        for(int i = 0; i < explosionAnimation.Length; i++){
            renderer.sprite = explosionAnimation[i];
              yield return new WaitForSeconds(0.1f);
        }
        GameObject.Destroy(gameObject);
            // if(explosionFrameCounter < 16) {
            //     renderer.sprite = explosionAnimation[explosionFrameCounter];
            //     explosionFrameCounter++;
            // }
            // else{
            //     explosionFrameCounter = 0;
            // }

    }
    // Update is called once per frame
    // void Update()
    // {
    //     timer += Time.deltaTime;
    //     if(timer < 1) {
    //         rb.velocity = new Vector2(0, -2);
    //     }
    //     else if (timer < 2) {
    //         rb.velocity = Vector2.zero;
    //         renderer.color = Color.red;
    //     }
    //     else {
    //         renderer.color = Color.white;
    //         transform.localScale = new Vector3(3,3,1);
    //         if(explosionFrameCounter < 16) {
    //             renderer.sprite = explosionAnimation[explosionFrameCounter];
    //             explosionFrameCounter++;
    //         }
    //         else{
    //             explosionFrameCounter = 0;
    //         }

    //     }
    // }
}
