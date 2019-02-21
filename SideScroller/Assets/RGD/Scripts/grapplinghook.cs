using UnityEngine;

public class grapplinghook : MonoBehaviour
{


    //grappling hook variables
    public LineRenderer line;
    DistanceJoint2D joint;
    Vector3 target;
    Vector2 targetPos;
    Vector2 transformConvert;
    RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    public float speed = 10.0f;
    private float step = 0.0f;
    public bool grappled = false;

    //gun additions
    public GameObject bullets;
    public float bulletSpeed;
    public float reloadTime;
    public bool reloading;

    private float reload;
    public HingeJoint hJoint;
    float hingeLength = 0f;
    Rigidbody body;
    public float transitionLength = 1f;
    float transitionStartTime = 0f;



    // Use this for initialization
    void Start()
    {
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetMouseButtonDown(0))
        if (Input.GetMouseButtonDown(1))
        {
            if (!grappled)
            {
                Grapple();
            }
        }

        if (grappled && Input.GetKeyDown(KeyCode.E))
        {
            MoveTo();
        }

        if (Input.GetMouseButtonDown(1))
        {
            //line.SetPosition(0, transform.position);
        }

        if (Input.GetMouseButtonUp(1))
        {
            line.enabled = false;
            grappled = false;
            Destroy(hJoint.gameObject.GetComponent<HingeJoint>());
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

    void Grapple()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Terrain") // if hit is terrain then create joint and set information
            {
                target = hit.transform.position;
                grappled = true;

                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);
                hit.transform.gameObject.AddComponent<HingeJoint>();
                hJoint = hit.transform.gameObject.GetComponent<HingeJoint>();
                hJoint.connectedBody = GetComponent<Rigidbody>();
                hJoint.anchor = transform.InverseTransformPoint(hit.collider.transform.position);
                hingeLength = hJoint.anchor.magnitude;
                hJoint.axis = transform.InverseTransformDirection(Vector3.forward);
                hJoint.autoConfigureConnectedAnchor = false;
                hJoint.massScale = 50;
            }

            //line.GetComponent<roperatio>().grabPos = hit.point;
        }
    }

    void MoveTo()
    {
        step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        if(Vector3.Distance(transform.position, target) <1.0f)
        {
            target = transform.position;
        }
    }
    //void Update()
    //{
    //    float step = speed * Time.deltaTime;

    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //        //Vector conversion
    //        transformConvert = transform.position;
    //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        hit = Physics2D.Raycast(transform.position, ray.direction, distance, mask);
    //        target = hit.point;

    //        if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
    //        {
    //            transform.position = Vector2.MoveTowards(transformConvert, target, step);

    //            line.enabled = true;
    //            line.SetPosition(0, transform.position);
    //            line.SetPosition(1, hit.point);
    //            //line.GetComponent<roperatio>().grabPos = hit.point;
    //        }

    //    }

    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        line.SetPosition(0, transform.position);
    //    }

    //    if (Input.GetKeyUp(KeyCode.E))
    //    {
    //        line.enabled = false;
    //        targetPos = Vector2.zero;
    //    }
    //}
    private void OnGUI()
    {

        GUI.Label(new Rect(20, 40, 300, 20), "x: " + target.x + "Y: " + target.y);
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
