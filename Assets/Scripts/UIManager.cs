using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour {
	[SerializeField] Dropdown _dropDownCommand;
	[SerializeField] Button _btnCommit;
	[SerializeField] Text _txtOrder;
	[SerializeField] Text _txtInfo;
	[SerializeField] GameObject _portraitGroupA;
	[SerializeField] GameObject _portraitGroupB;
	[SerializeField] GameObject _hpGroupA;
	[SerializeField] GameObject _hpGroupB;
	[SerializeField] RawImage _rImageCommonEffect;
	Action<int> _commandCheckCB;
	int _commandIndex;
	List<RawImage> _myPortraitGroup = new List<RawImage>();
	List<RawImage> _opponentPortraitGroup = new List<RawImage>();
	List<Slider> _myHpGroup = new List<Slider>();
	List<Slider> _opponentHpGroup = new List<Slider>();
	Dictionary<int, RawImage> _portraitDic = new Dictionary<int, RawImage>();
	Dictionary<int, Slider> _hpDic = new Dictionary<int, Slider>();

	public static UIManager Instance{
		get{
			return _instance;
		}
	}
	static UIManager _instance;

	void Awake(){
		_instance = this;
		InitUI();
	}

	public void InitUI(){
		_dropDownCommand.ClearOptions();
		_dropDownCommand.onValueChanged.AddListener(OnCommandChecked);
		var btnEvent = new Button.ButtonClickedEvent();
		btnEvent.AddListener(OnOKClicked);
		_btnCommit.onClick = btnEvent;
		AddComponentToList(_myPortraitGroup, _portraitGroupA.transform);
		AddComponentToList(_opponentPortraitGroup, _portraitGroupB.transform);
		AddComponentToList(_myHpGroup, _hpGroupA.transform);
		AddComponentToList(_opponentHpGroup, _hpGroupB.transform);
	}

	void AddComponentToList<T>(List<T> list_, Transform parent_){
		foreach(Transform child in parent_){
			T com  = child.GetComponent<T>();
			if(null == com){
				Debug.LogError("com is not found !!");
				continue;
			}
			child.gameObject.SetActive(false);
			list_.Add(com);
		}
	}

	public void InitPortraits(Dictionary<int, string> portraitDic_, int group){
		int curIndex = 0;
		foreach(var pair in portraitDic_){
			if(group == 0){
				_myPortraitGroup[curIndex].gameObject.SetActive(true);
				ResourcesManager.Instance.LoadTexture("Images/" + pair.Value, (tex)=>{
					_myPortraitGroup[curIndex].texture = tex;
				});
				_portraitDic[pair.Key] = _myPortraitGroup[curIndex];
				curIndex += 1;
			}else{
				_opponentPortraitGroup[curIndex].gameObject.SetActive(true);
				ResourcesManager.Instance.LoadTexture("Images/" + pair.Value, (tex)=>{
					_opponentPortraitGroup[curIndex].texture = tex;
				});
				_portraitDic[pair.Key] = _opponentPortraitGroup[curIndex];
				curIndex += 1;
			}
		}
	}

	public void ShowDamage(int id_){
		_portraitDic[id_].color = Color.red;
//		StartCoroutine(CoroutineShowDamage(id_));
	}

	public void ShowHp(int id_, float percent){
		_hpDic[id_].value = percent;
	}

	public void InitHps(List<int> idList_, int group){
		int curIndex = 0;
		foreach(var id in idList_){
			if(group == 0){
				_myHpGroup[curIndex].gameObject.SetActive(true);
				_myHpGroup[curIndex].value = 1f;
				_hpDic[id] = _myHpGroup[curIndex];
				curIndex += 1;
			}else{
				_opponentHpGroup[curIndex].gameObject.SetActive(true);
				_opponentHpGroup[curIndex].value = 1f;
				_hpDic[id] = _opponentHpGroup[curIndex];
				curIndex += 1;
			}
		}
	}

	public void ShowCurrent(int id_){
		_portraitDic[id_].color = Color.white;
//		StartCoroutine(CoroutineShowCurrent(id_));
	}
		
	public void ResetPortraits(){
		foreach(var pair in _portraitDic){
			pair.Value.color = Color.gray;
		}
	}

	IEnumerator CoroutineShowCurrent(int id_){
		_portraitDic[id_].color = Color.white;
		yield return new WaitForSeconds(0.5f);
		_portraitDic[id_].color = Color.gray;
	}

	IEnumerator CoroutineShowDamage(int id_){
		_portraitDic[id_].color = Color.red;
		yield return new WaitForSeconds(0.5f);
		_portraitDic[id_].color = Color.gray;
	}

	public void ShowCommands(List<string> commandList_, Action<int> cb_){
		_dropDownCommand.ClearOptions();
		_dropDownCommand.AddOptions(commandList_);
		_commandCheckCB = cb_;
	}

	public void ShowOrders(List<string> nameList_){
		string output = "";
		for(int i = nameList_.Count - 1; i > -1; i--){
			output += nameList_[i] + "\n";
		}
		_txtOrder.text = output;
	}

	public void OnOKClicked(){
		if(null != _commandCheckCB){
			_commandCheckCB(_commandIndex);
		}
		_commandCheckCB = null;
	}

	public void ShowInfo(string info_){
		_txtInfo.text = info_;
	}


	void OnCommandChecked(int val_){
		_commandIndex = val_;
	}

	public void ShowEffect(string path_){
		ResourcesManager.Instance.LoadTexture(path_, (tex)=>{
			_rImageCommonEffect.texture = tex;
		});
		StartCoroutine(CoroutineShowEffect());
	}

	IEnumerator CoroutineShowEffect(){
		_rImageCommonEffect.color = Color.clear;
		while(true){
			Color c = _rImageCommonEffect.color;
			c.a += Time.deltaTime;
			_rImageCommonEffect.color = c;
			if(c.a > 0.99){
				break;
			}
			yield return null;
		}
		_rImageCommonEffect.color = Color.white;
		yield return new WaitForSeconds(0.5f);
		_rImageCommonEffect.color = Color.clear;
	}
}
