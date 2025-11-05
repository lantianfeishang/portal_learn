using UnityEngine;

public class CameraRotarion : MonoBehaviour
{
    private float sumX_ratation = 0;//水平旋转总和
    private float sumY_ratation = 0;
    [SerializeField] float mouseSpeed;
    public Transform playerCamera;//摄像头

    public Transform father;//玩家与摄像头的父类

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //水平旋转
        sumX_ratation += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
        sumX_ratation %= 360;
        father.rotation = Quaternion.Euler(new(0, sumX_ratation, 0));

        //垂直旋转
        sumY_ratation += -Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;
        sumY_ratation = Mathf.Clamp(sumY_ratation, -10f, 45f);//限制y角度
        playerCamera.localRotation = Quaternion.Euler(new(sumY_ratation, playerCamera.localEulerAngles.y, playerCamera.localEulerAngles.z));
    }
}
