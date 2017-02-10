using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum TalkerPos{
	Left = 0,
	Right,
	Mid,
	Length,
}

public class ChatUIContent{
	public string name;
	public string portraitPath;
	public string content;
}

public class ChatUI : MonoBehaviour {
	[SerializeField] RawImage _rimageLeftPortrait;
	[SerializeField] RawImage _rimageRightPortrait;
	[SerializeField] RawImage _rimageMidPortrait;
	[SerializeField] Text _textContent;
	[SerializeField] Button _btnContinue;
	[SerializeField] GameObject _root;
	List<ChatUIContent> _contentList;
	int _contentIndex = 0;
	Dictionary<TalkerPos, RawImage> _portraitDic = new Dictionary<TalkerPos, RawImage>();
	Dictionary<string, TalkerPos> _portraitMatchDic = new Dictionary<string, TalkerPos>();
	public static ChatUI Instance;

	void Awake(){
		if(null == Instance){
			Instance = this;
		}else{
			Debug.LogError("More Than One ChatUI Exist");
		}
		InitUI();
	}

	void InitUI(){
		var e = new Button.ButtonClickedEvent();
		e.AddListener(Continue);
		_btnContinue.onClick =  e;

		_portraitDic.Add(TalkerPos.Left, _rimageLeftPortrait);
		_portraitDic.Add(TalkerPos.Right, _rimageRightPortrait);
		_portraitDic.Add(TalkerPos.Mid, _rimageMidPortrait);
	}

	public void StartTalking(string title, List<ChatUIContent> contenList_){
		Reset();
		int pos = 0;
		foreach(var content in contenList_){
			if(!_portraitMatchDic.ContainsKey(content.name)){
				_portraitMatchDic[content.name] = (TalkerPos) pos;
				if(pos < (int)TalkerPos.Mid){
					pos += 1;
				}
			}
		}
		_contentList = contenList_;
		Continue();
	}
		
	void Continue(){
		if(_contentIndex >= _contentList.Count){
			Finish();
			return;
		}
		var chatContent = _contentList[_contentIndex];
		_contentIndex += 1;
		ShowContent(chatContent);
	}

	void ShowContent(ChatUIContent chatContent){
		_textContent.text = chatContent.content;
		foreach(var pair in _portraitDic){
			pair.Value.color = Color.gray;
		}
		var image = _portraitDic[_portraitMatchDic[chatContent.name]];
		image.gameObject.SetActive(true);
		image.color = Color.white;
		ResourcesManager.Instance.LoadTexture(chatContent.portraitPath, (tex)=>{
			image.texture = tex;
		});
	}

	void Finish(){
		_root.SetActive(false);
	}

	void Reset(){
		_root.SetActive(true);
		_contentIndex = 0;
		_portraitMatchDic.Clear();
		_contentList = null;
		foreach(var pair in _portraitDic){
			pair.Value.gameObject.SetActive(false);
		}
	}
}
