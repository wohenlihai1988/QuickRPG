using UnityEngine;
using System.Collections.Generic;

public class TestChatUI : MonoBehaviour {
	
	void Start(){
		List<ChatUIContent> list = new List<ChatUIContent>();
		ChatUIContent content1 = new ChatUIContent();
		content1.name = "A";
		content1.portraitPath = "Images/a";
		content1.content = "i'm a";
		ChatUIContent content2 = new ChatUIContent();
		content2.name = "B";
		content2.portraitPath = "Images/b";
		content2.content = "i'm B";
		ChatUIContent content3 = new ChatUIContent();
		content3.name = "C";
		content3.portraitPath = "Images/c";
		content3.content = "i'm C";
		ChatUIContent content4 = new ChatUIContent();
		content4.name = "D";
		content4.portraitPath = "Images/a";
		content4.content = "i'm d";
		list.Add(content1);
		list.Add(content2);
		list.Add(content3);
		list.Add(content4);
			
		ChatUI.Instance.StartTalking(TestRes.Instance.GetString(), list);
	}

}
