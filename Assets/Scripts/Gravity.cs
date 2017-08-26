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
            else
                _mass = value;
        }
    }

    [SerializeField]
    private float _mass = 1; //1 is "earth size"

    /*
     * gravity is G(m1 * m2) / (dist * dist_relative_to_transform)^2 -> but since dist is relative and G is a constant
     * 
     * ehh....screw all this...stupid math
     */
    public readonly float GRAVITATIONAL_MULTIPLIER = Mathf.Pow(6.7f, -11f); //gravitational multiplier
    public readonly float MASS_MULTIPLIER = Mathf.Pow(5.98f, 24f); //mass of earth
    public readonly float DISTANCE_DEFAULT = Mathf.Pow(6.38f, 6); //sea level distance
    //TODO:Uh fix this constant. It is the decay of gravity(mulitplies the distance away from the center of earth
    private const float DISTANCE_RELATIVE = 5000f; //relative 

    //LOL
    public float slingshotMultiplier
    {
        get { return _slingshotMultiplier; }
        set { if (value < 0) _slingshotMultiplier = 0; else _slingshotMultiplier = value; }
    }
    [SerializeField]
    private float _slingshotMultiplier = 0.33f; //LOL


    private static Dictionary<GameObject, Rigidbody2D> orbitalsTable; //static meh...figure out later

    private List<GameObject> inOrbitTable;

    private CircleCollider2D trigger;

    // Use this for initialization
    private void Start()
    {
        if (trigger == null)
        {
            trigger = GetComponent<CircleCollider2D>();
        }
        //initialize orbitals in a list
        //TODO: Uh...scrap it? Change it to if object is inside the collider...do later
        if (orbitalsTable == null)
        {
            orbitalsTable = new Dictionary<GameObject, Rigidbody2D>();
            GameObject[] orbitals = GameObject.FindGameObjectsWithTag("Orbital");
            foreach (GameObject orbital in orbitals)
            {
                Rigidbody2D body = orbital.GetComponent<Rigidbody2D>();
                if (body != null)
                    orbitalsTable.Add(orbital, body);
            }
        }
        //initialize inOrbitTable;
        inOrbitTable = new List<GameObject>();

        //test achievement register
        Achievement.instance.Register(gameObject.name + "Gravity", TestEvent);
    }

    //If it is in the trigger it gets add to inorbit for gravity or something effect
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject orbital = collision.gameObject;
        if (orbitalsTable.ContainsKey(orbital) && !inOrbitTable.Contains(orbital))
        {
            inOrbitTable.Add(orbital);
        }
    }

    //take it out of it.
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject orbital = collision.gameObject;
        if (orbitalsTable.ContainsKey(orbital) && inOrbitTable.Contains(orbital))
        {
            inOrbitTable.Remove(orbital);
        }
    }

    private void Update()
    {
        Achievement.instance.Trigger(gameObject.name + "Gravity");
    }

    //Do all of our gravity here since it's physics.
    private void FixedUpdate()
    {
        //change this later to orbital
        foreach (GameObject orbital in inOrbitTable)
        {
            Rigidbody2D body = orbitalsTable[orbital];
            //apply force thingy
            Vector2 direction = transform.position - orbital.transform.position;

            //original gravity
            float force = GRAVITATIONAL_MULTIPLIER * MASS_MULTIPLIER * mass / Mathf.Pow(direction.magnitude * DISTANCE_RELATIVE + DISTANCE_DEFAULT, 2);
            body.AddForce(direction.normalized * force, ForceMode2D.Force);

            //slingshot stupidness
            body.AddForce(direction * slingshotMultiplier, ForceMode2D.Impulse);
        }
    }

    //Test Achievement event
    private void TestEvent(){
        Debug.Log(gameObject.name + " achieve trigger. LOL WUT!");
    }
}
