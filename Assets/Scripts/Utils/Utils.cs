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

    /// <summary>
    /// Draw a rectangle in 2D
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size">complete size of the rectangle</param>
    /// <param name="color"></param>
    public static void DebugDrawColoredRectangle(Vector3 center, Vector3 size, Color color) {
        center.x -= size.x / 2;
        center.y += size.y / 2;
        Debug.DrawLine(center, new Vector3(center.x + size.x, center.y, center.z), color);
        Debug.DrawLine(center, new Vector3(center.x, center.y - size.y, center.z), color);
        Debug.DrawLine(new Vector3(center.x, center.y - size.y, center.z), new Vector3(center.x + size.x, center.y - size.y, center.z), color);
        Debug.DrawLine(new Vector3(center.x + size.x, center.y - size.y, center.z), new Vector3(center.x + size.x, center.y, center.z), color);
    }

    /// <summary>
    /// Check parameters for the sanity of warning logs
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    public static bool AnimatorContainsParam(Animator anim, string paramName) {
        foreach (AnimatorControllerParameter param in anim.parameters) {
            if (param.name == paramName) return true;
        }
        return false;
    }
}
