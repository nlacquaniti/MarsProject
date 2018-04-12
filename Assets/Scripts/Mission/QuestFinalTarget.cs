using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestFinalTarget : MonoBehaviour
{

    private static QuestFinalTarget _Instance;
    private bool _Trigger = true;
    private static Image _Icon;
    private static MeshRenderer _MainMesh;
    private static MeshRenderer _SecondMesh;
    private static ParticleSystem _Particle;
    private static int _ChildCount = 0;

    private void Start()
    {
        _Instance = this;
        _Icon = Resources.Load<Image>("MinimapIcons/FinalTarget");
        _MainMesh = GetComponent<MeshRenderer>();
        

        _MainMesh.enabled = false;

        _ChildCount = transform.childCount;
        if (transform.childCount > 0)
        {
            _Particle = GetComponent<ParticleSystem>();
            _SecondMesh = transform.GetChild(0).GetComponent<MeshRenderer>();
            _Particle.Stop();
            _SecondMesh.enabled = false;
        }

        _Icon.enabled = false;
        MiniMap.instance.GetIcon(_Icon, _Instance.gameObject, true);
    }

    public static IEnumerator ShowFinalTarget()
    {

        while (_MainMesh == null && _Instance == null)
        {
            yield return null;
        }

        _Instance.StartCoroutine(MiniMap.instance.SetIconVisible(_Instance.gameObject));
        
        _MainMesh.enabled = true;
        if (_ChildCount > 0)
        {
            _Particle.Play();
            _SecondMesh.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vehicle") && Quest.currentQuest.CanBeCompleted && _Trigger)
        {
            _Trigger = false;
            StartCoroutine(HUD.instance.ShowObjectiveBox());
            Quest.currentQuest.ChangeMissionStatus(Quest.MissionStatus.COMPLETED);
        }
    }
}
