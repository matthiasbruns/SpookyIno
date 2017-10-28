using UnityEngine;

public static class LayerMaskExtensions
{
	public static bool IsInMask(this LayerMask obj, int layer) {
          return obj == (obj | (1 << layer));
	}
	public static bool IsNotInMask(this LayerMask obj, int layer) {
          return !IsInMask(obj, layer);
	}
}