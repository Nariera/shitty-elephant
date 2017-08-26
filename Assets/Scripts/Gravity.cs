using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    //what we define as mass
    public float mass
    {
        get
        {
            return _mass;
        }
        set
        {
            if (value < 0)
            {
                _mass = 0.01f; //maybe new default
            }
            _mass = value;
        }
    }

    [SerializeField]
    private float _mass = 1; //1 is "earth size"

    /*
     * gravity is G(m1 * m2) / (dist * dist_relative_to_transform)^2 -> but since dist is relative and G is a constant
     */
    public readonly float GRAVITATIONAL_MULTIPLIER = Mathf.Pow(6.7f, -11f); //gravitational multiplier
    public readonly float MASS_MULTIPLIER = Mathf.Pow(5.98f, 24f); //mass of earth
    public readonly float DISTANCE_DEFAULT = Mathf.Pow(6.38f, 6); //sea level distance

    //TODO:Uh fix this constant. It is the decay of gravity(mulitplies the distance away from the center of earth
    private const float DISTANCE_RELATIVE = 1f; //relative 

    //private const float GRAVITY_PULL_THRESHOLD = 0f; //do not apply gravity if the object if it is below this to improve performance


    private static List<Rigidbody2D> orbitalsTable; //static since there is no reason 

    // Use this for initialization
    private void Start()
    {
        //initialize orbitals in a list
        //TODO: Uh...scrap it?
        if (orbitalsTable == null)
        {
            orbitalsTable = new List<Rigidbody2D>();
            GameObject[] orbitals = GameObject.FindGameObjectsWithTag("Orbital");
            foreach (GameObject orbital in orbitals)
            {
                try
                {
                    Rigidbody2D body = orbital.GetComponent<Rigidbody2D>();
                    orbitalsTable.Add(body);

                }
                catch
                {
                    //do nothing...this shouldn't error here
                    Debug.LogError("Something went wrong with orbital initialization in gravity.");
                }
            }
        }

    }

    // Update is called once per frame
    private void Update()
    {
        //we do a periodic update to add and remove orbitals in it's threshold gravitational pull;
    }

    //Do all of our gravity here
    private void FixedUpdate()
    {
        //change this later to orbital in threshold
        foreach (Rigidbody2D orbital in orbitalsTable)
        {
            ExertGravity(orbital);
        }
    }

    //Exert gravity on objects
    private void ExertGravity(Rigidbody2D orbital)
    {
		Vector2 direction = transform.position - orbital.transform.position;
        float force = GRAVITATIONAL_MULTIPLIER * MASS_MULTIPLIER * mass / Mathf.Pow(direction.magnitude * DISTANCE_RELATIVE + DISTANCE_DEFAULT,2);
        orbital.AddForce(direction.normalized * force,ForceMode2D.Force);

    }

}
