using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D body;

    public float DownVelocity;
    // Use this for initialization

    public float turnSpeed
    {
        get
        {
            return _turnSpeed;
        }
        set
        {
            _turnSpeed = value;
        }
    }
    [SerializeField]
    private float _turnSpeed = 1;
    public float fartSpeed
    {
        get { return _fartSpeed; }
        set { _fartSpeed = value; }
    }
    [SerializeField]
    private float _fartSpeed = 2;

    //forward vector
    public Vector3 forward
    {
        get
        {
            if (_forward == null)
            {
                _forward = Vector3.right;
            }
            return _forward;
        }
        set
        {
            _forward = value;
        }
    }
    [SerializeField]
    private Vector3 _forward;
    private void Start()
    {
        Debug.Log("Test");
        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
        body.AddForce(new Vector2(0, DownVelocity), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool isFarting = Input.GetKey(KeyCode.Space);
        //rotate!
        if(horizontal != 0){
			transform.Rotate(new Vector3(0, 0, -horizontal * turnSpeed)); //negative due to rotation
		}
        if (isFarting)
        {
            Vector3 currentForward = Quaternion.Euler(0, 0, transform.root.eulerAngles.z) * forward;
            body.AddForce(currentForward * fartSpeed, ForceMode2D.Impulse);
        }

        //Debug stuff
        if(Input.GetKeyDown(KeyCode.Z)){
            body.velocity = Vector3.zero;
        }
    }
}
