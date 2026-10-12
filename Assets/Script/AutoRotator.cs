using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    // 回転速度（秒間30度）
    public float rotateSpeed = 30.0f; 
    void Update()
    {
        // Y軸（縦軸）を中心に回す
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
