using System;
using UnityEngine;

namespace ARena
{
	public class Plattform
	{
		public static bool IsMobile()
		{
			bool isMobile = false;
			#if UNITY_ANDROID
				isMobile = true;
			#endif

			#if UNITY_IOS
				isMobile = true;
			#endif

			return isMobile;
		}

		public static bool isMobileInEditor()
		{
			return Application.isEditor && IsMobile ();
		}

		public static bool isMobileInStandalone()
		{
			return !Application.isEditor && IsMobile ();
		}

		public static bool isAR(){
			// TODO
			return false;
		}
	}
}