using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    private Camera thisCamera;
    [SerializeField] Transform portalTransform;//传送门的方位
    [SerializeField] Transform cube;//人物的方位
    private void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }
/*    private float cameraY;//摄像头位置的y值
    private void OnEnable()
    {
        cameraY = thisCamera.transform.position.y;
    }
    //是否主角看着传送门
    private bool IfCubeNotLook()
    {
        float angle = Vector3.Angle(portalTransform.forward, cube.forward);
        //人物与门同方向，即不看
        if (angle < 90)
        {
            return true;
        }
        return false;
    }*/
    private void Update()
    {
/*        //如果在背后的相机，随人物运动而转动
        if (IfCubeNotLook())
        {
            Vector3 vector = cube.position;
            vector.y = cameraY;
            transform.LookAt(vector);
        }*/

        Plane p = new(portalTransform.forward, portalTransform.position + portalTransform.forward * 0.01f);//获取近裁切面，稍微加一丢丢距离保证近裁切面一定在传送门前方
        Vector4 clipPlane = new(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
        Matrix4x4.Transpose(Matrix4x4.Inverse(thisCamera.worldToCameraMatrix)) * clipPlane;//用逆转置矩阵将平面从世界空间变换到相机空间
        var newMatrix = thisCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);//计算并返回倾斜的近裁切面投影矩阵
        thisCamera.projectionMatrix = newMatrix;
    }
}
