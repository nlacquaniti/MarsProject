using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private Image _Icon;
    

    private void Start()
    {
        _Icon = Resources.Load<Image>("MinimapIcons/TargetIcon");
        
        _Icon.enabled = true;
        MiniMap.instance.GetIcon(_Icon, this.gameObject, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            
            TargetCollection.instance.TargetReached();
            transform.position = new Vector3(0,-10000,0);
            print("Got it");
            StartCoroutine(MiniMap.instance.RemoveIcon(this.gameObject));
        }
    }
}
