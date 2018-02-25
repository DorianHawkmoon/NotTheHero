using UnityEngine;

[CreateAssetMenu(fileName = "Random Wave", menuName = "Waves/Behaviour/Exponential")]
public class ExponentialData : BehaviourData {
    private TypeWave type = TypeWave.ExponentialNumber;
    public override TypeWave GetTypeBehaviour() {
        return type;
    }
}
