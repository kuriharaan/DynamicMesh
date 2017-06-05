using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSelector : MonoBehaviour
{
    [SerializeField]
    Platform platform;

    private void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(platform);
    }

    void Update ()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            int panelId = platform.GetPanelId(hit.point);
            platform.HilightPanel(panelId);

            if( 0 <= panelId )
            {
                if (Input.GetMouseButtonDown(0))
                {
                    platform.GainHeight(panelId);
                }
            }
        }
    }
}
