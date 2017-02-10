using UnityEngine;
using System.Collections;

public class Skill {
	
	public virtual float Calculate(Actor atk_, Actor def_){
		var damage = atk_.Attack - def_.Defence;
		def_.HP -= damage;
		def_.HP = Mathf.Max(0, def_.HP);
		return damage;
	}

	public virtual void ShowEffect(){
		
	}

	public virtual IEnumerator Process(){
		yield return new WaitForSeconds(1f);
	}

	public virtual string GetName(){
		return "Attack";
	}
}
