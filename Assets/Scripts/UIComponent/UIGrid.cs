using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIGrid : MonoBehaviour {

	[SerializeField] float _width;
	[SerializeField] float _height;
	[SerializeField] bool _vertical;
	[SerializeField] int _rowCount = 1;
	[SerializeField] bool _run;

	void Start(){
		Reset();
	}

	void Reset(){
		var rectTrans = GetComponent<RectTransform>();
		if(null != rectTrans){
			var size = rectTrans.sizeDelta;
			if(_vertical){
				size.y = transform.childCount * _height;
			}else{
				size.x = transform.childCount * _width;
			}
			rectTrans.sizeDelta = size;
		}
		int count = 0;
		Vector3 offset = new Vector3(-rectTrans.sizeDelta.x + _width * 0.5f, -rectTrans.sizeDelta.y + _height * 0.5f, 0);
		var pos = Vector3.zero;
		foreach(Transform child in transform){
			if(_vertical){
				pos.x = (count % _rowCount) * _width;
				pos.y = (count / _rowCount) * _height;
			}else{
				pos.x = (count / _rowCount) * _width;
				pos.y = (count % _rowCount) * _height;
			}
			count += 1;
			child.localPosition = pos + offset;
		}
	}

	void Update(){
		if(_run){
			Reset();
			_run = false;
		}
	}
}
