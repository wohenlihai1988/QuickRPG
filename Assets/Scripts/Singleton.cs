using UnityEngine;
using System.Collections;

public class Singleton<T> where T : new(){
	public static T Instance{
		get{
			if(null == _instance){
				_instance = new T();
			}
			return _instance;
		}
	}
	static T _instance;
}
