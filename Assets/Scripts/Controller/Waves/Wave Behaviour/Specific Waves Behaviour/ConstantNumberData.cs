using UnityEngine;

[CreateAssetMenu(fileName = "Random Wave", menuName = "Waves/Behaviour/Constant")]
public class ConstantNumberData : BehaviourData {
    private TypeWave type = TypeWave.ConstantNumber;

    public override TypeWave GetTypeBehaviour() {
        return type;
    }
}
