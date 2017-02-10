using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	Queue<int> _orderQueue = new Queue<int>();
	Dictionary<int, Actor> _actorDic = new Dictionary<int, Actor>();
	Dictionary<int, int> _curOrderDic = new Dictionary<int, int>();
	readonly int _maxQueueLength = 10;
	int _curActorID;

	void Start(){
		InitMyTeam();
		InitOpponentTeam();
		InitOrders();
		CalculateOrders();
		Dictionary<int, string> portraitsa = new Dictionary<int, string>();
		Dictionary<int, string> portraitsb = new Dictionary<int, string>();
		List<int> idListA = new List<int>();
		List<int> idListB = new List<int>();
		foreach(var pair in _actorDic){
			if(pair.Value.Team == ActorTeam.MySelf){
				portraitsa.Add(pair.Key, pair.Value.Name);
				idListA.Add(pair.Key);
			}else{
				portraitsb.Add(pair.Key, pair.Value.Name);
				idListB.Add(pair.Key);
			}
		}
		UIManager.Instance.InitPortraits(portraitsa, 0);
		UIManager.Instance.InitPortraits(portraitsb, 1);
		UIManager.Instance.InitHps(idListA, 0);
		UIManager.Instance.InitHps(idListB, 1);
		StartCoroutine(MainLoop());
		UIManager.Instance.ShowEffect("begin");
	}

	void InitMyTeam(){
		var a = new Actor();
		a.RandomAttribe();
		a.ID = 1001;
		a.Name = "a";
		a.Team = ActorTeam.MySelf;
		a.Skills.Add(new Attack());
		a.Skills.Add(new Kill());

		var b = new Actor();
		b.RandomAttribe();
		b.ID = 1002;
		b.Name = "b";
		b.Team = ActorTeam.MySelf;
		b.Skills.Add(new Skill());
		b.Skills.Add(new Attack());
		b.Skills.Add(new Kill());

		_actorDic.Add(a.ID, a);
		_actorDic.Add(b.ID, b);
	}

	void InitOpponentTeam(){

		var a = new Actor();
		a.ID = 2001;
		a.Name = "c";
		a.Team = ActorTeam.Opponent;
		a.Skills.Add(new Skill());
		a.RandomAttribe();

		var b = new Actor();
		b.RandomAttribe();
		b.ID = 2002;
		b.Name = "d";
		b.Team = ActorTeam.Opponent;
		b.Skills.Add(new Skill());

		_actorDic.Add(a.ID, a);
		_actorDic.Add(b.ID, b);
	}

	int GetNextActor(){
		var actorNext = 0;
		var lastTick = float.MaxValue;
		foreach(var orderPair in _curOrderDic){
			var nextTick = _actorDic[orderPair.Key].Tick * orderPair.Value;
			if(nextTick < lastTick){
				lastTick = nextTick;
				actorNext = orderPair.Key;
			}
		}
		_curOrderDic[actorNext] += 1;
		return _actorDic[actorNext].ID;
	}

	void InitOrders(){
		foreach(var actor in _actorDic){
			_curOrderDic.Add(actor.Value.ID, 0);
		}
	}

	void CalculateOrders(){
		for(int i = 0; i < _maxQueueLength; i++){
			_orderQueue.Enqueue(GetNextActor());
		}
	}

	int GetCurActor(){
		_orderQueue.Enqueue(GetNextActor());
		_curActorID = _orderQueue.Dequeue();
		return _curActorID;	
	}

	void PrintCurQueue(){
		List<string> outPut = new List<string>();
		foreach(var pair in _orderQueue){
			outPut.Add(_actorDic[pair].Name);
		}
		UIManager.Instance.ShowOrders(outPut);
	}

	void PrintCurActorInfo(){
		
	}

	List<string> GetSkillOptions(){
		var curActor = _actorDic[_curActorID];
		List<string> result = new List<string>();
		foreach(var skill in curActor.Skills){
			result.Add(skill.GetName());
		}
		return result;
	}

	int GetTarget(ActorTeam team_){
		foreach(var actorPair in _actorDic){
			if(actorPair.Value.Team == team_ && actorPair.Value.HP > 0){
				return actorPair.Value.ID;
			}
		}
		return 0;
	}

	bool CheckResult(){
		bool win = true;
		foreach(var pair in _actorDic){
			if(pair.Value.Team != ActorTeam.MySelf && pair.Value.HP > 0){
				win = false;
			}
		}
		return win;
	}

	IEnumerator MainLoop(){
		while(true){
			PrintCurQueue();
			var curActor = _actorDic[GetCurActor()];
			if(curActor.HP < 0){
				continue;
			}
			if(CheckResult()){
				break;
			}
			UIManager.Instance.ResetPortraits();
			UIManager.Instance.ShowCurrent(curActor.ID);
			switch(curActor.Team){
			case ActorTeam.MySelf:
				Skill curSkill = null;
				var opponent = _actorDic[GetTarget(ActorTeam.Opponent)];
				var damage = 0f;
				UIManager.Instance.ShowCommands(GetSkillOptions(), (index)=>{
					damage = curActor.Skills[index].Calculate(curActor, opponent);
					UIManager.Instance.ShowDamage(opponent.ID);
					curSkill = curActor.Skills[index];
				});
				while(true){
					if(null != curSkill){
						break;
					}else{
						yield return null;
					}
				}
				string info = curActor.Name + " use skill " + curSkill.GetName() + " to " + opponent.Name + " make " + damage + " damage " + "\n";
				foreach(var pair in _actorDic){
					info += pair.Value.Name + " : " + pair.Value.MaxHP + "\n";
				}
				UIManager.Instance.ShowInfo(info);
				yield return curSkill.Process();
				break;
			case ActorTeam.Friend:
				yield return curActor.Skills[0].Process();
				break;
			case ActorTeam.Opponent:
				yield return curActor.Skills[0].Process();
				break;
			}
			foreach(var pair in _actorDic){
				UIManager.Instance.ShowHp(pair.Key, pair.Value.HP / pair.Value.MaxHP);
			}
		}
		UIManager.Instance.ShowInfo("Win");
	}
}
