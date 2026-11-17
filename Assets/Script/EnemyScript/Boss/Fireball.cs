using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed;      // 飛ぶスピード
    [SerializeField] private float lifeTime;   // 消滅までの時間

    void Start()
    {
        // 一定時間経ったら自動で消える
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 毎フレーム、前方に進む
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}