using UnityEngine;
using System.Collections;

public class Combat {
	public static Combat Instance{
		get{
			if(null == _instance){
				_instance = new Combat();
			}
			return _instance;
		}
	}

	public static Combat _instance;

	public float Calculate(Actor atk_, Actor def_, Skill skill_){
		return 0f;
	}
}