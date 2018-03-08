using UnityEngine;

[CreateAssetMenu(fileName = "Constant Wave", menuName = "Waves/Behaviour/Constant")]
public class ConstantNumberData : BehaviourData {
    private TypeWave type = TypeWave.ConstantNumber;

    public int constantNumberWaves;

    public override TypeWave GetTypeBehaviour() {
        return type;
    }
}
