using UnityEngine;

public class grapplinghookSpring : MonoBehaviour
{


    //grappling hook variables
    public LineRenderer line;
    DistanceJoint2D joint;
    Vector3 target;
    Vector2 targetPos;
    Vector2 transformConvert;
    //RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    public float speed = 10.0f;
    private float step = 0.0f;
    private bool grappled = false;

    //3d grapplehook variables
    HingeJoint hJoint;
    RaycastHit hit;
    float hingeLength = 0f;
    Rigidbody body;
    public float transitionLength = 1f;
    public float transitionStartTime = 0f;

    //gun additions
    public GameObject bullets;
    public float bulletSpeed;
    public float reloadTime;
    public bool reloading;

    private float reload;
    public float jointAngle;



    // Use this for initialization
    void Start()
    {
        //line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Input.GetMouseButtonDown(0))
        if (Input.GetMouseButtonDown(1))
        {    
            Grapple();
        }

        if (grappled) // if grappled check if user wants to move up rope
        {
            if(Input.GetKeyDown(KeyCode.E))
                MoveTo();
        }       

        if (Input.GetKey(KeyCode.E))
        {
            //line.SetPosition(0, transform.position);
        }

        if (Input.GetMouseButtonUp(1))
        {
            //line.enabled = false;
            Destroy(hJoint);
            grappled = false;
            gameObject.GetComponent<PlayerMove>().enabled = true;
            gameObject.GetComponent<CharacterMotor>().sidescroller = true;
            gameObject.GetComponent<DoubleJumpEnabler>().enabled = true;
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }

        //gun stuff
        if (Input.GetMouseButtonDown(0))
        {
            if (!reloading)
            {
                fireGun();
                reloading = true;
            }
        }

        if (reloading)
        {
            reload += Time.deltaTime;

            if (reload >= reloadTime)
            {
                reload = 0.0f;
                reloading = false;
            }
        }
    }

    void FixedUpdate()
    {
    }

    void Grapple()
    {
        // create ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 3d raycast and send result to hit
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Terrain") // if hit is terrain then create joint and set information
            {
                target = hit.transform.position;
                grappled = true;
                gameObject.GetComponent<PlayerMove>().enabled = false;
                gameObject.GetComponent<CharacterMotor>().sidescroller = false;
                gameObject.GetComponent<DoubleJumpEnabler>().enabled = false;
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;

                hJoint = gameObject.AddComponent<HingeJoint>();
                hJoint.connectedBody = hit.transform.GetComponent<Rigidbody>();
                hJoint.anchor = transform.InverseTransformPoint(hit.collider.transform.position);

                jointAngle = hJoint.angle;
            }
        }
    }

    void MoveTo()
    {
        // attempt to add velocity to the players body towards where the grapple hook hit
        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(hit.transform.position.x, hit.transform.position.y, 0);
        
    }
    
    private void OnGUI()
    {

        GUI.Label(new Rect(20, 40, 300, 20), "x: " + target.x + "Y: " + target.y);
        //GUI.Label(new Rect(40, 40, 300, 20), hit.transform.position.ToString());
    }

    void fireGun()
    {
        GameObject newShot = Instantiate(bullets);

        Vector3 direction = Input.mousePosition - transform.position;
        Vector3 startPoint = transform.forward.normalized;
        newShot.transform.position = transform.position + startPoint;

        newShot.SetActive(true);
        newShot.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
    }
}
