using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TMP_Text text;
    
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        text.text = fps.ToString("000") + " FPS";
    }
}
