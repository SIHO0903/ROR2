using UnityEngine;

public class GlaiveFire : MonoBehaviour
{
    public float glaiveSpeed;
    public float glaiveRotSpeed;
    public float damage;
    Vector3 dirPos; //ù Ÿ���õ� ��
    Transform rotGlaive;
    Rigidbody rigid;

    [SerializeField] float maxBounces;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rotGlaive = transform.GetChild(0).GetComponent<Transform>();
    }
    private void OnEnable()
    {
        maxBounces = 6;
    }
    public void Dir(Vector3 startPos, Vector3 endPos, float damage)
    {
        dirPos = endPos - startPos;

        this.damage = damage;
    }
    // �������� �ִ�6�� ƨ���
    // ������ �Ÿ����� ���尡��� ������ ƨ���
    // �������� �ι������� �ȵ�
    // ���� �Ѹ��̸� �ѹ� ƨ��� ��
    private void Update()
    {
        rotGlaive.transform.Rotate(-Vector3.forward * glaiveRotSpeed * Time.deltaTime);
        transform.LookAt(dirPos);

    }

    private void FixedUpdate()
    {
        rigid.velocity = dirPos.normalized * glaiveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("����");
            if (maxBounces > 0)
            {
                Vector3 newDir = SearchNearestEnemy(other.transform);
                if (newDir != Vector3.zero)
                {

                    dirPos = newDir;
                    maxBounces--;
                }
                else
                {
                    gameObject.SetActive(false); 
                }
            }
            else
            {
                gameObject.SetActive(false); 
            }
        }
    }
    Vector3 SearchNearestEnemy(Transform excludeEnemy)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Infinity,LayerMask.GetMask("Enemy"));

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Debug.Log("������ ��" + colliders.Length);
        foreach (var collider in colliders)
        {
            if(collider.transform != excludeEnemy.transform)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform.GetChild(0).transform;
                    //Debug.Log(distance + " / ");
                }
            }
        }

        if (closestEnemy != null)
        {
            Debug.Log(closestEnemy.name);
            return (closestEnemy.position - transform.position);
        }

        return Vector3.zero;
    }
}
