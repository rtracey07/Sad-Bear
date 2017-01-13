using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	[Tooltip("Material that the camera rendering will be dumped into")]
	public Material mat;

	[Tooltip("Main Camera")]
	private Camera camera;

	void OnEnable () {
		
		camera = this.GetComponent<Camera> ();

		//Set Depth Texture to include normals. Used in the BufferedOrb shader
		//for determining intersects.
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}

	//Render a screen effect. Here is where we apply our grayscale material
	//to the all objects in the screen, based on alpha value.
	void OnRenderImage (RenderTexture source, RenderTexture destination){
		Graphics.Blit(source,destination,mat);
	}

}