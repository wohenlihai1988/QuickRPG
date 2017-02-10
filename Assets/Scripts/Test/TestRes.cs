using UnityEngine;
using System.Collections;

public class TestRes : Singleton<TestRes> {
	
	public string GetString(){
		return null;	
	}

	public int GetInt(){
		return int.MinValue;
	}

	public float GetFloat(){
		return float.MinValue;
	}

}
