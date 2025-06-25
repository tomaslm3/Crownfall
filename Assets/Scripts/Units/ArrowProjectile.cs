using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
    public float speed = 10f;
    private Vector3 targetPosition;
    private BaseUnit targetUnit;
    private int damage;

    public void Init(Vector3 start, BaseUnit target, int damage) {
        Debug.Log($"Iniciando flecha hacia {target.unitName} con daño {damage}");
        transform.position = start;
        targetUnit = target;
        targetPosition = target.transform.position;
        this.damage = damage;
    }

    void Update() {
        if (targetUnit == null) {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            Debug.Log($"Flecha alcanzó a {targetUnit.unitName} infligiendo {damage} de daño.");
            targetUnit.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }
}