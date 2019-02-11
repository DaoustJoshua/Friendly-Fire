using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickUp : MonoBehaviour {

    public float lifeSpan;
    public float currentLifeTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= lifeSpan)
        {
            DestroyObject(this.gameObject);
        }

        
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Untagged" || collision.transform.tag == "Player")
        {
            DestroyObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            DestroyObject(this.gameObject);
        }
    }
}
