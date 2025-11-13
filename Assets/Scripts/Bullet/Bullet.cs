using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timer;
    private float delTime;//计时器
    public Portal portal;//传送门
    public Portal portal_other;//另一个传送门
    private readonly float distance = 4;//两门距离

    public Mouse mouse;//反馈回射击脚本
    private void Awake()
    {
        timer = 2;
        delTime = 0;
    }
    private void Update()
    {
        if (delTime < timer)
        {
            delTime += Time.deltaTime;
            return;
        }
        mouse.hasbullet = false;
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wall"))
        {
            mouse.hasbullet = false;
            Destroy(gameObject);
            return;
        }

        //确定传送门朝向
        Transform wall = collision.gameObject.transform;
        float angle_protalAndWall = Vector3.Angle(transform.forward, wall.forward);
        Transform portalTransform = wall.transform;//记录传送门的未来朝向
        //相同朝向取反，否则同
        if (angle_protalAndWall < 90)
        {
            portalTransform.rotation = Quaternion.LookRotation(-wall.forward);
        }

        //与已存在的对立门近，且不为反向
        Vector3 hitPosition = collision.contacts[0].point;//碰撞点
        if (portal_other.gameObject.activeSelf && Vector3.Angle(portalTransform.forward, portal_other.transform.forward) < 90 && (portal_other.transform.position - hitPosition).magnitude < distance)
        {
            mouse.hasbullet = false;
            Destroy(gameObject);
            return;
        }

        portal.gameObject.SetActive(true);
        portal.transform.SetPositionAndRotation(hitPosition, portalTransform.rotation);
        mouse.ifShoutBulltX = !mouse.ifShoutBulltX;
        mouse.hasbullet = false;
        Destroy(gameObject);
    }
}
