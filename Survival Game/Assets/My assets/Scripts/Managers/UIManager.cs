using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool isLosePanelOpen;
    public bool isWinPanelOpen;

    public GameObject losePanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isLosePanelOpen = false;
        isWinPanelOpen = false;
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

}
