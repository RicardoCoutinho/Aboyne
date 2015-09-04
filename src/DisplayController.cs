
using UnityEngine;
using System.Collections;

public class DisplayController : MonoBehaviour {
	
	public GameObject player1Prefab;
	public GameObject player2Prefab;
	public GameObject goal1;
	public GameObject goal2;

	public GameObject ui;
	public GameObject map;
	
	public UIController uiController;
	
	void Start() {
		ui = GameObject.Find ("UI");
		uiController = ui.GetComponent<UIController>();
	}

	Vector2 GetHexSize(){
		GameObject Temp;

		Temp = GetComponent<FX_HexGen>().MakeHex(30, 0.5f);

		Bounds bounds = Temp.GetComponent<Renderer>().bounds;
		Vector2 extent = new Vector2(bounds.extents.x, bounds.extents.z);
		
		Destroy (Temp);
		
		return extent;
	}

	public void DisplayBoard(GameState state, Transform map){//Generate a map with the pointy part of the Hex on top

		this.map = map.gameObject;

		int[][] matrix = state.getBoard();

		Vector2 extent = GetHexSize();
		float ballRadius = player1Prefab.transform.localScale.x / 2.0f;

		float PosX = 0.0f;
		float PosY = 0.0f;
		float CamY = 0.0f;

		for(int y=0; y<state.getHeight(); y++)
		{
			for(int x=0; x<state.getWidth(); x++)
			{
				
				if ( matrix[y][x]>-1 )
				{
					PosX = 1.0f * extent.x * x;

					CamY = state.getHeight() - y - 1;
					PosY = 1.5f * extent.y * CamY;

					Transform h = GetComponent<FX_HexGen>().MakeHex(30,0.5f).transform;

					SetHexInfo(x,y,h);
					
					h.position = new Vector3(PosX, 0, PosY);
					//Debug.Log(map);
					h.parent = map.transform;

					h.gameObject.AddComponent<HexController>();
					h.gameObject.AddComponent<ObjectInfo>();
					h.GetComponent<ObjectInfo>().column = x;
					h.GetComponent<ObjectInfo>().line = y;
				
					GameObject b;

					switch (matrix[y][x]) // bolas
					{
						case -1 : break;
						case  0 : break;
						case  1 : 
						b = (GameObject) Instantiate(player1Prefab, new Vector3(PosX, ballRadius, PosY), Quaternion.identity);
						b.name = ("player1 ball");
						b.GetComponent<ObjectInfo>().column = x;
						b.GetComponent<ObjectInfo>().line = y;
						b.transform.parent = h;
						break;
						case  2 : 
						b = (GameObject) Instantiate(player2Prefab, new Vector3(PosX, ballRadius, PosY), Quaternion.identity);
						b.name = ("player2 ball");
						b.GetComponent<ObjectInfo>().column = x;
						b.GetComponent<ObjectInfo>().line = y;
						b.transform.parent = h;
						break;
						case  3 : 
						b = (GameObject) Instantiate(goal1, new Vector3(PosX, 0, PosY), Quaternion.identity);
						b.name = ("player1 goal");
						b.transform.parent = h;
						break;
						case  4 : 
						b = (GameObject) Instantiate(goal2, new Vector3(PosX, 0, PosY), Quaternion.identity);
						b.name = ("player2 goal");
						b.transform.parent = h;
						break;

					
					}
				}

			}
		}

	}

	public void EraseBoard() {
		if (map != null)
			Destroy(map);
	}
	
	void SetHexInfo(int x, int y, Transform h){
		
		h.GetComponent<FX_HexInfo>().HexPosition = new Vector3(x, 0, y);
		
		h.name = ("( Linha: " + y.ToString() + ", Coluna: " + x.ToString() + " )");
	}

}
