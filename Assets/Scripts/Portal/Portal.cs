using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal portal_other;//另一个传送门
    private void OnCollisionEnter(Collision collision)
    {
        if(!portal_other.gameObject.activeSelf)
        {
            return;
        }
        //方块
        if (collision.gameObject.CompareTag("Cube"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 vector = rb.velocity;
            vector.z = -vector.z;
            rb.velocity = vector;
            collision.gameObject.transform.SetPositionAndRotation(portal_other.transform.position + portal_other.transform.forward * 2, Quaternion.LookRotation(portal_other.transform.forward));

            Cube cubeScript = collision.gameObject.GetComponent<Cube>();
            cubeScript.hasIntoPortal = !cubeScript.hasIntoPortal;
            return;
        }
        //玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraRotarion cameraRotarion = collision.gameObject.GetComponent<CameraRotarion>();
            cameraRotarion.LookTransform(portal_other.transform);

            Quaternion quaternion = collision.gameObject.transform.rotation;
            quaternion.x = 0;
            quaternion.z = 0;
            collision.gameObject.transform.SetPositionAndRotation(portal_other.transform.position + portal_other.transform.forward * 1, quaternion);
        }
    }
}
