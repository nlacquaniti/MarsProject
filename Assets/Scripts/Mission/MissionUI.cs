using UnityEngine;

//[CreateAssetMenu(menuName = "Project Mars Tools / Create Mission")]
public class MissionUI : ScriptableObject
{

    [SerializeField]
    private float[] _Gold = new float[4];
    [SerializeField]
    private float[] _Silver = new float[4];
    [SerializeField] 
    private float[] _Bronze = new float[4];
    [SerializeField]
    private QuestType[] _SelectedQuest = new QuestType[4];
    [SerializeField] 
    private bool[] _Created = new bool[4];

    public enum QuestType {None, SpecialDelivery, CollectInfos, FillOfEnergy, FreeRoaming};
    public bool[] IsMissionAvabile { get { return _Created; } set { _Created = value; } }
    public QuestType[] CurrentQuest { get { return _SelectedQuest; } set { _SelectedQuest = value; } }
    public float[] Gold { get { return _Gold; } set { _Gold = value; } }
    public float[] Silver { get { return _Silver; } set { _Silver = value; } }
    public float[] Bronze { get { return _Bronze; } set { _Bronze = value; } }
 
}




