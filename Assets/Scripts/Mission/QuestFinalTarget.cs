using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestFinalTarget : MonoBehaviour
{

    private static QuestFinalTarget _Instance;
    private bool _Trigger = true;
    private static Image _Icon;
    private static MeshRenderer _Mesh;

    private void Start()
    {
        _Instance = this;
        _Icon = Resources.Load<Image>("MinimapIcons/TargetIcon");
        _Mesh = GetComponent<MeshRenderer>();
        _Mesh.enabled = false;
        _Icon.enabled = false;
        MiniMap.instance.GetIcon(_Icon, _Instance.gameObject, true);
    }

    public static IEnumerator ShowFinalTarget()
    {

        while (_Mesh == null && _Instance == null)
        {
            yield return null;
        }

        _Instance.StartCoroutine(MiniMap.instance.SetIconVisible(_Instance.gameObject));
        _Mesh.enabled = true;
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
