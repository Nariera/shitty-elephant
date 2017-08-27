using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWin : MonoBehaviour
{
    private string PlanetName = "";

    private void Start()
    {
        PlanetName = transform.parent.transform.parent.name;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControl>() != null)
        {
            Achievement.instance.Trigger(PlanetName + " approached.");
        }
    }
}
