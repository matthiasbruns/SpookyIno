using UnityEngine;

public static class LayerMaskExtensions
{
	public static bool HasLayer(this LayerMask obj, int layer) {
          return obj == (obj | (1 << layer));
	}
	public static bool HasNotLayer(this LayerMask obj, int layer) {
          return !HasLayer(obj, layer);
	}
}