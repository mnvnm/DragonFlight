using UnityEngine;

public class GameSuccessDlg : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Init()
    {
        Show(false);
    }
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
