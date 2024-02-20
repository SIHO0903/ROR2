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
    private void Update()
    {
        //�۷��̺갡 ȸ���ϸ� ���󰡸� ���̰�
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
            if (maxBounces > 0)
            {
                Vector3 newDir = SearchSecondNearEnemy(other.transform);
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
    Vector3 SearchSecondNearEnemy(Transform excludeEnemy)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Infinity,LayerMask.GetMask("Enemy")); // ����� ����
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if(collider.transform != excludeEnemy.transform) //���尡��������� 
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform.GetChild(0).transform; //�ι�°�� �������
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
