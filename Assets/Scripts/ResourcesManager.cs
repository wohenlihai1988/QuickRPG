using UnityEngine;
using System.Collections.Generic;
using System;

class CacheContent{
	public string path;
	public Texture tex;
	public int count;
}

public class ResourcesManager : Singleton<ResourcesManager> {
	Texture _defaultTex;
	Dictionary<string, CacheContent> _cache = new Dictionary<string, CacheContent>();

	public ResourcesManager(){
		if(null == _defaultTex){
			_defaultTex = Resources.Load("default") as Texture;
		}
	}

	public void LoadTexture(string path_, Action<Texture> cb_){
		Texture tex = null;
		if(_cache.ContainsKey(path_)){
			_cache[path_].count += 1;
			cb_(_cache[path_].tex);
			return;
		}
		tex = Resources.Load(path_) as Texture;
		if(null == tex){
			tex = _defaultTex;
		}
		var content = new CacheContent();
		content.count = 0;
		content.path = path_;
		content.tex = tex;
		_cache[path_] = content;
		cb_(tex);
	}
}
