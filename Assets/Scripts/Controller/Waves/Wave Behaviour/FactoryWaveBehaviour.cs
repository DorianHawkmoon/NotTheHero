using UnityEngine;

public class FactoryWaveBehaviour {

    public static WaveBehaviour CreateWaveBehaviour(TypeWave typeWave, BehaviourData data, GameObject objectAttach) {
        WaveBehaviour wave = null;

        switch (typeWave) {
            case TypeWave.Random: {
                    RandomWave w = (RandomWave)objectAttach.AddComponent(typeof(RandomWave));
                    w.SetData(data);
                    wave = w;
                    break;
                }
            case TypeWave.ExponentialNumber: {
                    ExponentialWave w = (ExponentialWave)objectAttach.AddComponent(typeof(ExponentialWave));
                    w.SetData(data);
                    wave = w;
                    break;
                }
            case TypeWave.ConstantNumber: {
                    ConstantNumberWaves w = (ConstantNumberWaves)objectAttach.AddComponent(typeof(ConstantNumberWaves));
                    w.SetData(data);
                    wave = w;
                    break;
                }
        }

        return wave;
    }
}
