
public class ExponentialWave : WaveBehaviour {
    private ExponentialData data;

    public void SetData(BehaviourData data) {
        this.data = (ExponentialData)data;
    }
}
