using UnityEngine;

public class Utils  {

    /// <summary>
    /// Converts given bitmask to layer number
    /// </summary>
    /// <returns> layer number </returns>
    public static int ToLayer(int bitmask) {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1) {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }

    public static int ToLayer(LayerMask[] layers) {
        int layerCollision = 0;
        for (int i = 0; i < layers.Length; ++i) {
            int layer = Utils.ToLayer(layers[i]);
            layerCollision |= (1 << layer);
        }
        return layerCollision;
    }
}
