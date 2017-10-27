using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spooky {
	
	public static class VectorExtensions {
	
		public static Vector2 SetX(this Vector2 vec, float x){
			vec.x = x;
			return vec;
		}

		public static Vector2 SetY(this Vector2 vec, float y){
			vec.y = y;
			return vec;
		}

		public static Vector3 SetX(this Vector3 vec, float x){
			vec.x = x;
			return vec;
		}

		public static Vector3 SetY(this Vector3 vec, float y){
			vec.y = y;
			return vec;
		}

		public static Vector3 SetZ(this Vector3 vec, float z){
			vec.z = z;
			return vec;
		}

		public static Vector3 Copy(this Vector3 vec) {
			return new Vector3 (vec.x, vec.y, vec.y);
		}

		public static Vector2 Copy(this Vector2 vec) {
			return new Vector2 (vec.x, vec.y);
		}

		public static Vector3 AddXY(this Vector3 vec3, Vector2 vec2) {
			return new Vector3 (vec3.x + vec2.x, vec3.y + vec2.y, vec3.z);
		}

		public static Vector3 AddXZ(this Vector3 vec3, Vector2 vec2) {
			return new Vector3 (vec3.x + vec2.x, vec3.y, vec3.z + vec2.y);
		}

		public static Vector3 AddYZ(this Vector3 vec3, Vector2 vec2) {
			return new Vector3 (vec3.x, vec3.y + vec2.x, vec3.z + vec2.y);
		}
	}
}
