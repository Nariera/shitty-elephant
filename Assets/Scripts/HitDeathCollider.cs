using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitDeathCollider : MonoBehaviour {



	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        //We probably want this to check for general orbital objects if those will exist and can burn up.
        PlayerControl thePlayer = other.GetComponent<PlayerControl>();
        if(thePlayer != null)
        {
            thePlayer.gameObject.SetActive(false);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}
