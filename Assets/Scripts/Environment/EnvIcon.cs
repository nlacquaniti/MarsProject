using UnityEngine;

public class EnvIcon : MonoBehaviour
{
    private UnityEngine.UI.Image _EnvArea = null;
    private UnityEngine.UI.Image _EnvIcon = null;

    private const float SCALE_MULTIPLIER = 0.33f;

    private void Start()
    {
        var _EnvName = this.gameObject.GetComponent<EnvPropertyScript>().propertyType.ToString();

        _EnvArea = Resources.Load<UnityEngine.UI.Image>("MinimapIcons/" + _EnvName);
        _EnvArea.transform.localScale = new Vector3((SCALE_MULTIPLIER / 100) * this.transform.localScale.x, (SCALE_MULTIPLIER / 100) * this.transform.localScale.y, (SCALE_MULTIPLIER / 100) * this.transform.localScale.z );

        _EnvIcon = Resources.Load<UnityEngine.UI.Image>("MinimapIcons/" + _EnvName + "ICON");

        StartCoroutine(WaitingMinimap());
    }


    private System.Collections.IEnumerator WaitingMinimap()
    {
        while (MiniMap.instance == null)
        {
            yield return null;
        }

        MiniMap.instance.GetIcon(_EnvArea, this.gameObject, false);
        MiniMap.instance.GetIcon(_EnvIcon, this.gameObject, false);
    }


}


