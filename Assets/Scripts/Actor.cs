using UnityEngine;
using System.Collections.Generic;
public enum ActorTeam{
	MySelf,
	Friend,
	Opponent,
	Length,
}
// data class
public class Actor {

	public float Speed;
	public float Tick;
	public int ID;
	public string Name;
	public float MaxHP;
	public float MaxMP;
	public float HP;
	public float MP;
	public float Attack;
	public float Defence;
	public ActorTeam Team;
	public List<Skill> Skills = new List<Skill>();
	public string Portrait;

	public void RandomAttribe(){
		Speed = Random.Range(20, 30);
		MaxHP = Random.Range(50, 100);
		HP = MaxHP;
		MaxMP = Random.Range(100, 400);
		MP = MaxMP;
		Attack = Random.Range(50,100);
		Defence = Random.Range(0,5);
		Tick = 1f / Speed;
	}
}
