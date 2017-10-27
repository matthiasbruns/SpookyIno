using System;
using UnityEngine;

namespace ARena
{
	public class GameTime
	{
		public static float deltaTime {
			get {
				return Time.deltaTime;
			}
		}

		public static float fixedDeltaTime {
			get {
				return Time.fixedDeltaTime;
			}
		}
	}
}

