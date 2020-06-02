using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : Singleton<GUIManager>
{
    public GameObject prefabGUIOver;
    public GameObject parenGUI;

    public GameObject guiOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGUIOver()
    {
        if (guiOver == null)
        {
            guiOver = Instantiate(prefabGUIOver);
            guiOver.transform.parent = parenGUI.transform;
            guiOver.transform.localPosition = new Vector2(0, 0);
        }

        UIManager.Instance.ShowOne(guiOver.GetComponent<GUIGameOver>());
    }

    public void HideGUIAllGUI()
    {
        if (guiOver)
        {
            UIManager.Instance.SetClose(guiOver.GetComponent<GUIGameOver>());
        }
    }
}
