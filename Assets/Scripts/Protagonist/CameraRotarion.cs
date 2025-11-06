using UnityEngine;

public class CameraRotarion : MonoBehaviour
{
    public float x_ratation;//水平位移
    public float y_ratation;
    public float sumY_ratation;//垂直旋转总和
    [SerializeField] public float mouseSpeed;
    public Transform playerCamera;//摄像头

    
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
        y_ratation = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;
        sumY_ratation -= y_ratation;
        sumY_ratation = Mathf.Clamp(sumY_ratation, -45f, 45f);//限制y角度
        playerCamera.localRotation = Quaternion.Euler(new(sumY_ratation, playerCamera.localEulerAngles.y, playerCamera.localEulerAngles.z));
    }
    public void LookTransform(Transform transform)
    {
        this.transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
