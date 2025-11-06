using UnityEngine;

public class CameraRotarion : MonoBehaviour
{
    private float x_ratation;//水平位移
    private float sumY_ratation;//垂直旋转总和
    [SerializeField] float mouseSpeed;
    [SerializeField] Transform playerCamera;//摄像头
    
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
    public void LookTransform(Transform transform)
    {
        this.transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
