using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject spawnBullet;
    public string key;
    public int bulletCount;
    GameObject bulletAmountIcon;

    // Start is called before the first frame update
    void Start()
    {
        bulletAmountIcon = GameObject.Find("bulletAmount");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && bulletCount > 0){
            Instantiate(spawnBullet, transform.position + new Vector3(1.5f,0,0), Quaternion.identity);
            bulletAmountIcon.transform.position += new Vector3(2, 0, 0);
            bulletCount--;
            print(bulletCount);
        }
    }

    void OnTriggerEnter(Collider c) {   
        print("is not tag problem");
        if (c.gameObject.tag == "Gem") {
            if (bulletCount < 3){
                bulletCount++;
                bulletAmountIcon.transform.position -= new Vector3(2, 0, 0);
                print(bulletCount);
            }

        }
        GameObject.Destroy(c.gameObject);
    }

}
