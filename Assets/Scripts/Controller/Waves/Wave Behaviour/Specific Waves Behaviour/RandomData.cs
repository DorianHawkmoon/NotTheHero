using UnityEngine;

[CreateAssetMenu(fileName = "Random Wave", menuName = "Waves/Behaviour/Random")]
public class RandomData : BehaviourData {
    private TypeWave type = TypeWave.Random;

    public int maxRandomSpawn = 10;
    public int minRandomSpawn = 3;

    public override TypeWave GetTypeBehaviour() {
        return type;
    }
}
