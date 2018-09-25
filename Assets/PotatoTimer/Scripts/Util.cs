using UnityEngine.Events;

public static class Util{
	
	public static void InvokeSafe( this UnityEvent action) {
		if (action != null)
			action.Invoke();
	}
	
}
