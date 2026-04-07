using UnityEngine;

namespace AmazonCommon
{
	public abstract class AndroidJavaObjectWrapper
	{
		protected AndroidJavaObject javaObj;

		protected abstract string JAVA_CLASS_NAME { get; }

		protected AndroidJavaObjectWrapper()
		{
			try
			{
				javaObj = new AndroidJavaObject(JAVA_CLASS_NAME);
			}
			catch
			{
				Debug.LogError("Could not obtain java " + JAVA_CLASS_NAME + " class.");
			}
		}

		public AndroidJavaObject ToAndroidJavaObject()
		{
			return javaObj;
		}
	}
}
