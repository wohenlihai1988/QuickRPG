using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapGenerator{
	class Range{
		public int _min;
		public int _max;

		public Range(int min_, int max_){
			_min = min_;
			_max = max_;
		}
	}

	public class Vector2i{
		public int x;
		public int y;
		public Vector2i(int x_, int y_){
			x = x_;
			y = y_;
		}
		public override string ToString ()
		{
			return string.Format ("(" + x.ToString() + "," + y.ToString() + ")");
		}
	}

	class Zone{
		public int _index;
		public Range _hRange;
		public Range _vRange;

		public Zone(Range h_, Range v_){
			_hRange = h_;
			_vRange = v_;
		}
	}

	public class MapGenerator : MonoBehaviour {
		public int _zoneCount = 0;
		List<List<Zone>> _zoneList = new List<List<Zone>>();
		public bool _regenerate = false;
		int _width = 64;
		Range _pathLength = new Range(5, 20);
		Range _zoneWidth = new Range(10, 30);
		List<GameObject> _tiles = new List<GameObject>();
		void Start(){
			var root = new GameObject("MapRoot").transform;
			root.transform.position = new Vector3(-24, -30, 50);
			for(int i = 0; i < _width * _width; i++){
				var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = root;
				cube.transform.localPosition = new Vector3(i / _width, i % _width, 0f);
				cube.name = (i / _width).ToString() + "-" + (i % _width).ToString();
				_tiles.Add(cube);
			}
			Generate();
		}

		void Update(){
			if(_regenerate){
				Generate();
				_regenerate = false;
			}
		}

		void Generate(){
			RandomZones();
		}

		void RandomZones(){
			for(int i = 0; i < _tiles.Count; i++){
				_tiles[i].SetActive(true);
			}
			StartCoroutine(CoroutineCreateZone());
		}

		IEnumerator CoroutineCreateZone(){
			_zoneCount = 0;
			int zoneHStart = 0;
			int zoneHEnd = 0;
			int zoneVStart = 0;
			int zoneVEnd = 0;
			int lastZoneHEnd = 0;
			int lastZoneVEnd = 0;
			while(zoneHEnd < _width - 1){
				if(!LoopProtect.Instance.Run(0)){
					break;
				}
				zoneHStart = Random.Range(_pathLength._min, _pathLength._max); 
				zoneHEnd = zoneHStart + Random.Range(_zoneWidth._min, _zoneWidth._max);
				zoneHStart += lastZoneHEnd;
				zoneHEnd += lastZoneHEnd;
				zoneHEnd = Mathf.Min(zoneHEnd, _width - 1);
				lastZoneHEnd = zoneHEnd;
				List<Zone> zoneList = new List<Zone>();
				while( zoneVEnd < _width - 1){
					if(!LoopProtect.Instance.Run(1)){
						break;
					}
					zoneVStart = Random.Range(_pathLength._min,_pathLength._max);
					zoneVEnd = zoneVStart + Random.Range(_zoneWidth._min, _zoneWidth._max);
					zoneVStart += lastZoneVEnd;
					zoneVEnd += lastZoneVEnd;
					zoneVEnd = Mathf.Min(zoneVEnd, _width - 1);
					lastZoneVEnd = zoneVEnd;
					for(int i = zoneHStart; i < zoneHEnd; i++){
						for(int j = zoneVStart; j < zoneVEnd; j++){
							_tiles[i * _width + j].SetActive(false);
						}
					}
					if(zoneHStart < zoneHEnd && zoneVStart < zoneVEnd){
						zoneList.Add(new Zone(new Range(zoneHStart, zoneHEnd), new Range(zoneVStart, zoneVEnd)));
					}
					yield return new WaitForSeconds(1f);
					_zoneCount += 1;
				}
				if(zoneList.Count > 0){
					_zoneList.Add(zoneList);
				}
				zoneVEnd = 0;
				lastZoneVEnd = 0;
			}
			StartCoroutine(CoroutineRandomRoads());
		}

		IEnumerator CoroutineRandomRoads(){
			Zone lastHZone = null;
			Zone lastVZone = null;
			foreach(var list in _zoneList){
				if(null != lastHZone){
					var start = Random.Range(lastHZone._vRange._min, lastHZone._vRange._max);
					var end = Random.Range(list[0]._vRange._min, list[0]._vRange._max);
					GenerateRoad(new Vector2i(lastHZone._hRange._max, start), new Vector2i(list[0]._hRange._min, end));
					yield return new WaitForSeconds(1f);
					lastHZone = list[0];
				}
				lastHZone = list[0];
				foreach(var zone in list){
					if(null == lastVZone){
						lastVZone = zone;
						continue;
					}
					var start = Random.Range(lastVZone._hRange._min, lastVZone._hRange._max);
					var end = Random.Range(zone._hRange._min, zone._hRange._max);
					GenerateRoad(new Vector2i(start, lastVZone._vRange._max), new Vector2i(end, zone._vRange._min));
					yield return new WaitForSeconds(1f);
				}
				lastVZone = null;
			}
		}

		void GenerateRoad(Vector2i startPoint , Vector2i endPoint){
			Debug.LogError(startPoint.ToString() + "," + endPoint.ToString());
			Vector2i start = null;
			Vector2i end = null;
			if(startPoint.x < endPoint.x){
				start = startPoint;
				end = endPoint;
			}else{
				start = endPoint;
				end = startPoint;
			}
			int cross = (start.x + end.x) / 2;
			int j = start.y;

			for(int i = start.x; i <= end.x; i++){
				var back = false;
				var jChange = 0;
				if(i >= cross){
					if(end.y >= start.y && j < end.y){
						jChange = 1;
						back = true;
					}else if(end.y <= start.y && j > end.y){
						jChange = -1;
						back = true;
					}
				}
				if(i * _width + j > _tiles.Count - 1){
					Debug.LogError("nani ? !");
				}
				_tiles[i * _width + j].SetActive(false);
				if(back){
					i -= 1;
				}
				j += jChange;
			}
		}
	}
}
