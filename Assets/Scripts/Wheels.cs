using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Mars Tools / Wheels")]
public class Wheels : ScriptableObject {
    public float weight;
    public float iceAdherence;
    public float rockAdherence;
    public float sandAdherence;
    public int bonusSpeed;
    public int productivityMod;
    public int airManeuverability;
    public int groundManeuverability;

}
