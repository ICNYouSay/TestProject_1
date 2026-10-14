using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().alpha = (Mathf.Sin(Time.time) + 1) / 3 * 2;
    }
}
