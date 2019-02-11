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
    private bool grappled = false;

    //gun additions
    public GameObject bullets;
    public float bulletSpeed;
    public float reloadTime;
    public bool reloading;

    private float reload;

    

    // Use this for initialization
    void Start()
    {
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.E))
        {
            Grapple();
        }

        if (grappled)
        {
            MoveTo();
        }

        if (Input.GetKey(KeyCode.E))
        {
            line.SetPosition(0, transform.position);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            line.enabled = false;
            grappled = false;
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

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider.tag == "Terrain")
        {
            target = hit.transform.position;
            grappled = true;

            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hit.point);

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
        GameObject newShot = Instantiate(bullets, transform);

        Vector3 direction = Input.mousePosition - transform.position;

        newShot.SetActive(true);
        newShot.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
    }
}
