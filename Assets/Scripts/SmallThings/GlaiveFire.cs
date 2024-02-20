using UnityEngine;

public class GlaiveFire : MonoBehaviour
{
    public float glaiveSpeed;
    public float glaiveRotSpeed;
    public float damage;
    Vector3 dirPos; //첫 타겟팅된 적
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
        //글레이브가 회전하면 날라가며 보이게
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Infinity,LayerMask.GetMask("Enemy")); // 모든적 감지
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if(collider.transform != excludeEnemy.transform) //가장가까운적제외 
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform.GetChild(0).transform; //두번째로 가까운적
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
