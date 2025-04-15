using UnityEngine;

public class Blackhole2 : MonoBehaviour
{
    [Header("�ڶ�����")]
    public float maxSize = 5f;
    public float growSpeed = 2f;
    public float attractForce = 50f;
    public float blackholeDuration = 5f;

    [Header("�������")]
    public ForceMode2D forceMode = ForceMode2D.Force;
    public float rotationForce = 10f; // �����תЧ��

    private Rigidbody2D rb;
    private float currentSize;
    private float destroyTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSize = 0;
    }

    private void Update()
    {
        HandleGrowth();
        HandleLifetime();
    }

    private void FixedUpdate()
    {
        ApplyAttractionForces();
    }

    private void HandleGrowth()
    {
        if (currentSize < maxSize)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, growSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * currentSize;
        }
    }

    private void HandleLifetime()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= blackholeDuration)
        {
            // ��ֹͣ��������Ч��
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            Destroy(gameObject);
        }
    }

    private void ApplyAttractionForces()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            currentSize,
            LayerMask.GetMask("Enemy")
        );

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Enemy")) continue;

            Rigidbody2D targetRb = collider.GetComponent<Rigidbody2D>();
            if (targetRb == null) continue;

            Vector2 direction = (transform.position - collider.transform.position).normalized;
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            // ���ݾ��붯̬��������
            float dynamicForce = attractForce * (1 - distance / maxSize);

            // ʩ������������
            targetRb.AddForce(direction * dynamicForce, forceMode);

            // �����תЧ������ѡ��
            targetRb.AddTorque(rotationForce * Mathf.Sign(Vector2.Dot(direction, Vector2.up)));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ����������������ƶ�����������ʵ�֣�
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.FreezeTime(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ����ƶ�����
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.FreezeTime(false);
        }
    }

    private void OnDestroy()
    {
        // ����������Ӱ����˵�״̬
        var colliders = Physics2D.OverlapCircleAll(
            transform.position,
            maxSize,
            LayerMask.GetMask("Enemy")
        );

        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null) enemy.FreezeTime(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentSize);
    }
}