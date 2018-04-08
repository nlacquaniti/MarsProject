using UnityEngine;
public class Collectable : MonoBehaviour
{
    private MeshRenderer _Mesh;
    private bool _CollisionTrigger = true;


    private void Start()
    {
        _Mesh = GetComponent<MeshRenderer>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vehicle") && _CollisionTrigger)
        {
            AudioManager.Audio.PlayObjectivesSound(ObjectiveSounds.CollectAchivement);
            _CollisionTrigger = false;
            _Mesh.enabled = false;
            transform.position = new Vector3(0, -10000, 0);
            StartCoroutine(HUD.instance.ShowCollectableBox());
            Quest.currentQuest.CollectableFound = true;
            
        }
    }
}

