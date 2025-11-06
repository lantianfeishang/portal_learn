using UnityEngine;

public class Mouse : MonoBehaviour
{
    //射击有关
    [SerializeField] Transform bulletsPool;//子弹父级
    [SerializeField] Bullet Bullet_x;//子弹x,y
    [SerializeField] Bullet Bullet_y;
    [SerializeField] float bulletSpeed;//子弹速度
    public bool ifShoutBulltX;
    [SerializeField] float shoutTime;
    private float delTime;
    [SerializeField] Portal Portal_x;//传送门
    [SerializeField] Portal Portal_y;
    public bool hasbullet;//是否仍存在子弹
    [SerializeField] Transform mouseStar;//准星

    //举抛方块有关
    [SerializeField] Transform playerCamera;//摄像机
    private GameObject cube = null;
    [SerializeField] float max_distance;//方块距离
    [SerializeField] float smoothedSpeed;//方块追鼠标速度
    [SerializeField] float throwSpeed;//方块投掷速度
    [SerializeField] float cubeXSpeed;//脱离准星后方块速度
    [SerializeField] float cubeYSpeed;

    private Cube cubeScript;
    private Rigidbody thisRb;
    private Rigidbody cubeRb;
    private void Awake()
    {
        delTime = shoutTime;
        hasbullet = false;
        thisRb = GetComponent<Rigidbody>();
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
            hasbullet = true;
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
            //Vector3 direction = (mouseStar.position - playerCamera.transform.position).normalized;
            bulletPerfab.GetComponent<Rigidbody>().velocity = mouseStar.forward * bulletSpeed;
        }
        //投方块
        if(cube!= null && Input.GetMouseButtonDown(0))
        {
            cubeRb.velocity = (playerCamera.forward * throwSpeed) + thisRb.velocity * 0.5f;//投掷+惯性
            CleanCube();
        }
        //举方块
        if (Input.GetMouseButtonUp(1))
        {
            if (cube == null)
            {
                return;
            }
            cubeRb.velocity = thisRb.velocity * 0.5f;//惯性
            CleanCube();
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
                if (!hit.collider.gameObject.CompareTag("Cube"))
                {
                    return;
                }
                cube = hit.collider.gameObject;
                cubeRb = cube.GetComponent<Rigidbody>();
                cubeRb.useGravity = false;
                cubeRb.velocity = Vector3.zero;
                cubeScript = cube.GetComponent<Cube>();
                cubeScript.hasIntoPortal = false;
            }
        }
        if (Input.GetMouseButton(1))
        {
            if(cube == null)
            {
                return;
            }
            //如果穿过传送门
            if (cubeScript.hasIntoPortal)
            {
                Vector3 vector = new();
                vector += cubeXSpeed * Input.GetAxis("Mouse X") * transform.right;
                vector += cubeYSpeed * Input.GetAxis("Mouse Y") * transform.up;
                cube.transform.position += Time.deltaTime * vector;//因视角移动
                cubeRb.velocity = thisRb.velocity;//因位置移动
                return;
            }
            //未穿
            Vector3 rayOrgin = playerCamera.position;
            Vector3 cubePosition = rayOrgin + playerCamera.forward * max_distance;
            //cube.transform.position = Vector3.Lerp(cube.transform.position, cubePosition, smoothedSpeed * Time.deltaTime);
            cube.transform.position = cubePosition;
        }
    }
    private void CleanCube()
    {
        cubeRb.useGravity = true;
        cubeRb = null;
        cube = null;
        cubeScript = null;
    }
}
