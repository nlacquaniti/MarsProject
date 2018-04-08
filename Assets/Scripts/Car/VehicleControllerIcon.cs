using UnityEngine;

public class VehicleControllerIcon : MonoBehaviour
{
    private UnityEngine.UI.Image _Icon = null;

    private void Start()
    {
        _Icon = Resources.Load<UnityEngine.UI.Image>("MinimapIcons/VehicleIcon");
        _Icon.enabled = true;
        StartCoroutine(WaitingMinimap());
    }


    private System.Collections.IEnumerator WaitingMinimap()
    {
        while(MiniMap.instance == null)
        {
            yield return null;
        }

        MiniMap.instance.GetIcon(_Icon, this.gameObject, false);

    }
}
