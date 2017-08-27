using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    [SerializeField]
    private Rigidbody2D body;
    private Animator cameraAnim, mahAnim;
    public GameObject fartVFX;
    private List<ParticleSystem> fartParticles = new List<ParticleSystem>();
    public Image fartOMeter;
    [Range(0, 1)] public float currentFart;

    public float rateOfFartUse = 0.05f;
    public float rateOfFartGain = 0.01f;

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

    public List<SpriteRenderer> ListOfSprites;
    public bool IsSuperBoost = false;
    public float CurrentBoostTime = 0;


    [SerializeField]
    private Vector3 _forward;

    private Vector3 RespawnLocation;

    private bool Victory = false;

    private void Start()
    {

        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }
        cameraAnim = Camera.main.GetComponent<Animator>();
        mahAnim = GetComponent<Animator>();

        body.AddForce(new Vector2(0, DownVelocity), ForceMode2D.Impulse);
        RespawnLocation = transform.position;

        //"VictoryScreen"
        Achievement.instance.Register("VictoryScreen", Winning);
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool isKeyPressed = Input.GetKey(KeyCode.Space) || Input.GetAxis("Vertical") > 0;
        bool isFarting = isKeyPressed && currentFart > rateOfFartUse;
        //rotate!
        if (horizontal != 0)
        {
            body.angularVelocity = (-horizontal * turnSpeed * 10);
            //transform.Rotate(new Vector3(0, 0, -horizontal * turnSpeed)); //negative due to rotation
        }

        //Cam Rot
        cameraAnim.SetBool("Left", horizontal < 0);
        cameraAnim.SetBool("Right", horizontal > 0);

        //Farting VFX?
        foreach (var i in fartParticles)
        {
            var em = i.emission;
            em.enabled = isFarting;
        }

        //superboost superceded the key press stuff
        if (IsSuperBoost)
        {
            Debug.Log(CurrentBoostTime);
            if (CurrentBoostTime > 1.5)
            {
                Debug.Log("End Boost");
                CurrentBoostTime = 0;
                IsSuperBoost = false;
            }
            float boostSpeed = 5;
            Vector3 currentForward = Quaternion.Euler(0, 0, transform.root.eulerAngles.z) * forward;
            body.AddForce(currentForward * boostSpeed, ForceMode2D.Impulse);

            foreach (SpriteRenderer render in ListOfSprites)
            {
                if (CurrentBoostTime > 1.5)
                {
                    render.color = new Color(1, 1, 1);
                }
                else if (CurrentBoostTime > 1.35)
                {
                    render.color = new Color(1, .4f, .8f);
                }
                else if (CurrentBoostTime > 1.15)
                {
                    render.color = new Color(.75f, 1, .25f);
                }
                else if (CurrentBoostTime > 1)
                {
                    render.color = new Color(1, 0, 1);
                }
                else if (CurrentBoostTime > .85)
                {
                    render.color = new Color(0, 0, 1);
                }
                else if (CurrentBoostTime > .65)
                {
                    render.color = new Color(0, 1, 0);
                }
                else if (CurrentBoostTime > 0.5)
                {
                    render.color = new Color(1, 1, 0);
                }
                else if (CurrentBoostTime > 0.35)
                {
                    render.color = new Color(1f, .4f, 0);
                }
                else if (CurrentBoostTime > 0.15)
                {
                    render.color = new Color(1, 0, 0);
                }
                else
                {
                    render.color = new Color(1, 1, 1);
                }
            }

            CurrentBoostTime += Time.deltaTime;
        }

        //Physics && gas
        if (!isKeyPressed)
        {
            if (CameraTracking.nearestPlanetToPlayer != null)
            {
                var mag = (CameraTracking.nearestPlanetToPlayer.transform.position - transform.position).magnitude;

                var perc = Mathf.Clamp((100 - mag) / 100, 0.1f, 1);

                currentFart += rateOfFartGain * perc;
            }
            else
            {
                currentFart += rateOfFartGain;

            }
        }
        else if (currentFart > rateOfFartUse && !IsSuperBoost)
        {
            currentFart -= rateOfFartUse;
            Vector3 currentForward = Quaternion.Euler(0, 0, transform.root.eulerAngles.z) * forward;
            body.AddForce(currentForward * fartSpeed, ForceMode2D.Impulse);
        }
        else if(!IsSuperBoost)
        {
            foreach (var i in fartParticles)
            {
                if (i.name == "Stars")
                {
                    var em = i.emission;
                    em.enabled = true;
                }
            }

            Vector3 currentForward = Quaternion.Euler(0, 0, transform.root.eulerAngles.z) * forward;
            body.AddForce(currentForward * fartSpeed / 20, ForceMode2D.Impulse);

        }

        //Anim
        mahAnim.SetBool("Farting", isKeyPressed);

        currentFart = Mathf.Clamp01(currentFart);

        fartOMeter.fillAmount = currentFart;

        //Debug stuff
        //		if (Input.GetKeyDown(KeyCode.Z))
        //		{
        //			body.velocity = Vector3.zero;
        //		}
    }

    void OnEnable()
    {
        //Register farts
        foreach (var i in fartVFX.GetComponentsInChildren<ParticleSystem>())
        {
            fartParticles.Add(i);
        }

        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void Respawn()
    {
        Debug.Log("Respawn!");
        transform.position = RespawnLocation;
        gameObject.SetActive(true);
        //body.velocity = Vector2.zero;
        Gravity.AddOrbital(gameObject);
        GetComponent<PlayerCondition>().Reset();
        currentFart = 1;
    }

    private void Winning()
    {
        victoryScreen.SetActive(true);
        Victory = true;
    }

    void OnDisable()
    {
        if (Victory) return;
        gameOverScreen.SetActive(true);

    }

    //Boost you one way super fast
    public void SuperForwardBoost()
    {
        Debug.Log("Called");
        IsSuperBoost = true;
        CurrentBoostTime = 0f;
    }
}
