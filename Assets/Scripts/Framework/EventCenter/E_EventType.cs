namespace MarkFramework
{
	public enum E_EventType
	{
		/// <summary>
		/// monster die  - parameter : monster
		/// </summary>
		E_Monster_Dead,
		
		/// <summary>
		/// player gets reward  - parameter : int
		/// </summary>
		E_Player_Get_Reward,
		
		/// <summary>
		/// update progress - parameter : int 
		/// </summary>
		E_Progress_Update,
		
		/// <summary>
		/// key down - parameter : KeyCode 
		/// </summary>
		E_Key_Down,
		
		/// <summary>
		/// key up - parameter : KeyCode 
		/// </summary>
		E_Key_Up,
		
		/// <summary>
		/// UI elements' value change
		/// </summary>
		E_Raise_Property
		
	}
}

