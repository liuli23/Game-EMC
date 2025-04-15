using UnityEngine;

public class Blackhole2 : MonoBehaviour
{
    [Header("黑洞参数")]
    public float maxSize = 5f;
    public float growSpeed = 2f;
    public float attractForce = 50f;
    public float blackholeDuration = 5f;

    [Header("物理参数")]
    public ForceMode2D forceMode = ForceMode2D.Force;
    public float rotationForce = 10f; // 添加旋转效果

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
            // 先停止所有物理效果
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

            // 根据距离动态调整力度
            float dynamicForce = attractForce * (1 - distance / maxSize);

            // 施加线性吸引力
            targetRb.AddForce(direction * dynamicForce, forceMode);

            // 添加旋转效果（可选）
            targetRb.AddTorque(rotationForce * Mathf.Sign(Vector2.Dot(direction, Vector2.up)));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 立即冻结敌人自主移动（根据需求实现）
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.FreezeTime(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 解除移动锁定
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.FreezeTime(false);
        }
    }

    private void OnDestroy()
    {
        // 重置所有受影响敌人的状态
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