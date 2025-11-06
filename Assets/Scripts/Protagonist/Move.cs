using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rd;//本刚体
    //xy轴移动
    [SerializeField] public float speed;//速度
    public Vector3 y;//y轴移动量;
    //z轴移动
    private bool isGround = false;//是否不在空中
    [SerializeField] float jumpSpeed;//跳跃速度
    [SerializeField] float fallAcc;//坠落加速度
    [SerializeField] float fallInit;//坠落初速度
    [SerializeField] float fallMax;//坠落最大速
    [SerializeField] float jumpTime;//能跳多久
    private float fallSpeed = 0;//坠落速度
    private float delJumpTime;//计时器

    private void Awake()
    {
        rd = GetComponent<Rigidbody>();
        delJumpTime = jumpTime;//防止一进入就跳跃
    }
    private void FixedUpdate()
    {
        //x,y移动
        Vector3 moveDir = new();
        moveDir += transform.forward * Input.GetAxis("Vertical");
        y = moveDir += transform.right * Input.GetAxis("Horizontal");
        moveDir += y;
        moveDir *= speed;
        //不在跳跃
        if (isGround)
        {
            rd.velocity = moveDir;
            return;
        }
        //跳跃上升阶段
        if (delJumpTime < jumpTime)
        {
            delJumpTime += Time.deltaTime;
            moveDir.y = jumpSpeed;
            rd.velocity = moveDir;
            return;
        }
        //下降阶段
        if (fallSpeed < fallMax)
        {
            fallSpeed -= fallAcc * Time.deltaTime;
        }
        moveDir.y = fallSpeed;
        rd.velocity = moveDir;       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            delJumpTime = 0;
            fallSpeed = fallInit;
            isGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //触地
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //不触地
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}
