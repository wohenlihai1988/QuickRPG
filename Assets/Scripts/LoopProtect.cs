using UnityEngine;
using System.Collections.Generic;

public class LoopProtect : Singleton<LoopProtect> {
	Dictionary<int, int> _dic = new Dictionary<int, int>();

	public bool Run(int index_){
		if(_dic.ContainsKey(index_)){
			_dic[index_] += 1;
			if(_dic[index_] > 100){
				_dic[index_] = 0;
				return false;
			}
			return true;
		}else{
			_dic[index_] = 1;
			return true;
		}
	}
}
