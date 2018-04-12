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
        if (this.transform.parent == null)
        {
            _EnvArea.transform.localScale = new Vector3((SCALE_MULTIPLIER / 100) * this.transform.localScale.x, (SCALE_MULTIPLIER / 100) * this.transform.localScale.y, (SCALE_MULTIPLIER / 100) * this.transform.localScale.z);
        }
        else
        {   
            if(this.transform.parent.parent == null)
            {
                _EnvArea.transform.localScale = new Vector3((SCALE_MULTIPLIER / 100) * this.transform.localScale.x * this.transform.parent.localScale.x, (SCALE_MULTIPLIER / 100) * this.transform.localScale.y * this.transform.parent.localScale.y, (SCALE_MULTIPLIER / 100) * this.transform.localScale.z * this.transform.parent.localScale.z);
            }
            else
            {
                _EnvArea.transform.localScale = new Vector3((SCALE_MULTIPLIER / 100) * this.transform.localScale.x * this.transform.parent.localScale.x * this.transform.parent.parent.localScale.x, (SCALE_MULTIPLIER / 100) * this.transform.localScale.y * this.transform.parent.localScale.y * this.transform.parent.parent.localScale.y, (SCALE_MULTIPLIER / 100) * this.transform.localScale.z * this.transform.parent.localScale.z * this.transform.parent.parent.localScale.z);
            }
        }
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


