using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poptart : MonoBehaviour {

    public float ReduceRate = 0.005f;
    public ParticleSystem particle;
    private bool SetDestroy = false;
    private float timeDestroy = 0;
    private void Start(){
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
        if (player != false){
            player.rateOfFartUse -= ReduceRate;


            Vector3 directional = transform.position - collision.gameObject.transform.position;
			float angle = Vector3.Angle(Vector3.right, directional);
			
			transform.eulerAngles = new Vector3(0, 0, -angle);

			particle.Play();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            (GetComponent<SpriteRenderer>() as SpriteRenderer).enabled = false;
            SetDestroy = true;
        }
    }

    private void Update(){
        if(SetDestroy){
            if(timeDestroy > 0.5){
                particle.Stop();
            }
            if(timeDestroy > 1){
                
                Destroy(this);
            }
            timeDestroy += Time.deltaTime;
           
        }
    }
}
