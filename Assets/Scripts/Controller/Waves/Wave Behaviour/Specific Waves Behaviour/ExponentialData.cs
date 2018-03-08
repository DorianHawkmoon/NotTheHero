using UnityEngine;

[CreateAssetMenu(fileName = "Exponential Wave", menuName = "Waves/Behaviour/Exponential")]
public class ExponentialData : BehaviourData {
    private TypeWave type = TypeWave.ExponentialNumber;
    
    public int steps; //cuanto dura el proceso segundos (steps)
    public float beginValueTime; //en que valor empieza
    public float endValueTime; //en que valor quiero acabar
    public EasingEquationsDouble.EasingEquations timeFunction;
    public AnimationCurve curveTime = new AnimationCurve();
    
    public int initialNumber;
    public int finalNumber;
    public EasingEquationsDouble.EasingEquations numberFunction;
    public AnimationCurve curveNumber = new AnimationCurve();

    public override TypeWave GetTypeBehaviour() {
        return type;
    }
}
