using UnityEngine;

public class CameraRotarion : MonoBehaviour
{
    private float x_ratation;//水平位移
    private float sumY_ratation;//垂直旋转总和
    [SerializeField] float mouseSpeed;
    [SerializeField] Transform playerCamera;//摄像头

    //近物体调整摄像头
    [SerializeField] Transform target;//玩家
    private Vector3 offset = new(0, 3.5f, -5f); // 初始偏移
    [SerializeField] float smoothSpeed = 5f; // 平滑移动速度
    [SerializeField] float minDistance;
    private Vector3 smoothedPosition; // 平滑后的位置
    private void Awake()
    {
        sumY_ratation = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //水平旋转
        x_ratation = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
        transform.Rotate(Vector3.up * x_ratation);

        //垂直旋转
        sumY_ratation -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;
        sumY_ratation = Mathf.Clamp(sumY_ratation, -45f, 45f);//限制y角度
        playerCamera.localRotation = Quaternion.Euler(new(sumY_ratation, playerCamera.localEulerAngles.y, playerCamera.localEulerAngles.z));
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 射线检测：从玩家到期望位置发射射线，检测障碍物
        Vector3 rayOrigin = target.position; // 射线起点
        Vector3 rayDirection = target.up * offset.y; // 射线方向
        rayDirection += target.right * offset.z;
        float rayDistance = rayDirection.magnitude; // 射线长度

        // 发射射线（只检测指定层的障碍物）
        if (Physics.Raycast(rayOrigin, rayDirection, rayDistance, 1 << 6))
        {
            smoothedPosition = new Vector3(0, rayOrigin.y + 0.5f, 0);
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, smoothedPosition, smoothSpeed * Time.deltaTime);
            return;
        }
        Vector3 rayDirection_2 = target.up * offset.y;
        rayDirection_2 -= target.right * offset.z;
        //对称
        if (Physics.Raycast(rayOrigin, rayDirection_2, rayDistance, 1 << 6))
        {
            smoothedPosition = new Vector3(0, rayOrigin.y + 0.5f, 0);
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, smoothedPosition, smoothSpeed * Time.deltaTime);
            return;
        }
        smoothedPosition = offset;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, smoothedPosition, smoothSpeed * Time.deltaTime);
    }

    public void LookTransform(Transform transform)
    {
        this.transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
