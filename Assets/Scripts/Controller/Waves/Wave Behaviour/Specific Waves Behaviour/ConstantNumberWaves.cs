using System;

public class ConstantNumberWaves : WaveBehaviour {
    private ConstantNumberData data;
    public int constantNumberWaves; 
    
    public void SetData(BehaviourData data) {
        this.data = (ConstantNumberData)data;
    }
}
