using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Transform playerCamera;//摄像机
    private GameObject cube = null;
    [SerializeField] float max_distance;//方块距离
    [SerializeField] float smoothedSpeed;//方块追鼠标速度
    [SerializeField] float throwSpeed;//方块投掷速度

    public Transform bulletsPool;//子弹父级
    public Bullet Bullet_x;//子弹x,y
    public Bullet Bullet_y;
    [SerializeField] float bulletSpeed;//子弹速度
    public bool ifShoutBulltX;
    [SerializeField] float shoutTime;
    private float delTime;
    public GameObject Portal_x;//传送门
    public GameObject Portal_y;
    public bool hasbullet;//是否仍存在子弹
    public Transform mouseStar;//准星
    private void Awake()
    {
        delTime = shoutTime;
        hasbullet = false;
    }
    private void Update()
    {
        //射击
        if (delTime < shoutTime)
        {
            delTime += Time.deltaTime;
        }
        if (cube == null && Input.GetMouseButtonDown(0))
        {
            if (hasbullet)
            {
                return;
            }
            if(delTime < shoutTime)
            {
                return;
            }
            delTime = 0;
            Bullet bulletPerfab;
            if (ifShoutBulltX)
            {
                bulletPerfab = Instantiate(Bullet_y, mouseStar);
                bulletPerfab.portal = Portal_y;
                bulletPerfab.portal_other = Portal_x;
            }
            else
            {
                bulletPerfab = Instantiate(Bullet_x, mouseStar);
                bulletPerfab.portal = Portal_x;
                bulletPerfab.portal_other = Portal_y;
            }
            bulletPerfab.transform.SetParent(bulletsPool);
            bulletPerfab.mouse = this;
            Vector3 direction = mouseStar.position - playerCamera.position;
            bulletPerfab.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
        //投方块
        if(cube!= null && Input.GetMouseButtonDown(0))
        {
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = playerCamera.forward * throwSpeed;
            cube = null;
        }
        //举方块
        if (Input.GetMouseButtonUp(1))
        {
            if (cube == null)
            {
                return;
            }
            cube.GetComponent<Rigidbody>().useGravity = true;
            cube = null;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(cube != null)
            {
                return;
            }
            Vector3 rayOrgin = playerCamera.position;
            if (Physics.Raycast(rayOrgin, playerCamera.forward, out RaycastHit hit, 20, 1 << 7))
            {
                if (hit.collider.gameObject.CompareTag("Cube"))
                {
                    cube = hit.collider.gameObject;
                    Rigidbody rb = cube.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            if(cube == null)
            {
                return;
            }
            Vector3 rayOrgin = playerCamera.position;
            Vector3 cubePosition = rayOrgin + playerCamera.forward * max_distance;
            cube.transform.position = Vector3.Lerp(cube.transform.position, cubePosition, smoothedSpeed * Time.deltaTime);
        }
    }
}
