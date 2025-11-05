using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject portal;//传送门
    public GameObject portal_other;//另一个传送门
    private readonly float distance = 4;//两门距离

    public Mouse mouse;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 hitPosition = collision.contacts[0].point;
            if(portal_other.activeSelf && (portal_other.transform.position - hitPosition).magnitude < distance)
            {
                mouse.hasbullet = false;
                Destroy(gameObject);
                return;
            }
            portal.SetActive(true);
            mouse.ifShoutBulltX = !mouse.ifShoutBulltX;
            portal.transform.position = hitPosition;
        }
        mouse.hasbullet = false;
        Destroy(gameObject);
    }
}
