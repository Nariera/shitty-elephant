using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject Player;
    public Vector3 cameraOffset;

    // Use this for initialization
    private void Start()
    {
        if (cameraOffset == null)
        {
            cameraOffset = Vector3.zero;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Player != null)
        {
            //transform.rotation = Player.transform.rotation;

            transform.TransformPoint(cameraOffset);

            transform.rotation = Player.transform.rotation;
            //         Vector3 newPosition = Player.transform.position + cameraOffset;
            //         newPosition = Quaternion.Euler(0, 0, Player.transform.root.eulerAngles.z) * newPosition;
            //newPosition.z = transform.position.z;
            //transform.position = newPosition;
        }
    }
}
