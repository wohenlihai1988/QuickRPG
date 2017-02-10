using UnityEngine;
using System.Collections.Generic;

public class AttrMap : MonoBehaviour {
	
	public int _attrCount = 100;
	public float radius = 3.0f;
	public Color _centerColor = Color.white;
	public Color _bgColor = Color.blue;
	public Color _edgeColor = Color.red;
	public Color _lineColor = Color.white;
	List<float> _lengthList = new List<float>();

	void Start(){
		for(int i = 0; i < _attrCount; i++){
			_lengthList.Add(Random.Range(0.2f, 1f));
		}
	}

	static Material lineMaterial;
	static void CreateLineMaterial ()
	{
		if (!lineMaterial)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			Shader shader = Shader.Find ("Hidden/Internal-Colored");
			lineMaterial = new Material (shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt ("_ZWrite", 0);
		}
	}

	// Will be called after all regular rendering is done
	public void OnRenderObject ()
	{
		CreateLineMaterial ();
		lineMaterial.SetPass (0);

		GL.PushMatrix ();
		GL.LoadOrtho();

		// Draw lines
		GL.Begin (GL.TRIANGLES);
		var lastAngle = 0f;
		var lastLength = _lengthList[0];

		for (int i = 0; i < _attrCount + 1; ++i)
		{
			float a = i / (float)_attrCount;
			float angle = a * Mathf.PI * 2;
			GL.Color (_bgColor);
			var r = radius;
			GL.Vertex3 (0.5f + Mathf.Sin (lastAngle) * r, 0.5f + Mathf.Cos (lastAngle) * r, 0);
			GL.Vertex3 (0.5f + Mathf.Sin (angle) * r, 0.5f + Mathf.Cos (angle) * r, 0);
			GL.Vertex3(0.5f, 0.5f, 0f);
			lastAngle = angle;
		}
		for (int i = 0; i < _attrCount + 1; ++i)
		{
			float a = i / (float)_attrCount;
			float angle = a * Mathf.PI * 2;
			var r = _lengthList[i % _lengthList.Count] * radius;
			var c = _edgeColor * r;
			c.a = 1f;
			GL.Color (c);
			var lr = lastLength;
			GL.Vertex3 (0.5f + Mathf.Sin (lastAngle) * lr, 0.5f + Mathf.Cos (lastAngle) * lr, 0);
			GL.Vertex3 (0.5f + Mathf.Sin (angle) * r, 0.5f + Mathf.Cos (angle) * r, 0);
			GL.Color (_centerColor);
			GL.Vertex3(0.5f, 0.5f, 0f);
			lastAngle = angle;
			lastLength = r;
		}

		GL.End ();

		GL.Begin (GL.LINES);
		lastAngle = 0f;
		lastLength = _lengthList[0];

		for (int i = 0; i < _attrCount + 1; ++i)
		{
			float a = i / (float)_attrCount;
			float angle = a * Mathf.PI * 2;
			GL.Color (_lineColor);
			var r = radius;
			GL.Vertex3(0.5f, 0.5f, 0f);
			GL.Vertex3 (0.5f + Mathf.Sin (angle) * r, 0.5f + Mathf.Cos (angle) * r, 0);
			lastAngle = angle;
		}

		GL.End ();

		GL.PopMatrix ();
	}
}
