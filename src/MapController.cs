using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	
	public GameObject srcHex;
	public GameObject dstHex;

	public HexController srcHexController;
	public HexController dstHexController;

	public GameState gameState;

	public bool srcSelected = false;
	public bool dstSelected = false;

	public bool refresh = false;

	// Use this for initialization
	void Start () {

		srcHex = null;
		srcHexController = null;

		dstHex = null;
		dstHexController = null;
	}

	public void selection(GameObject g) {

		if ((srcHex != null && dstHex != null) || (srcHex == null && dstHex == null)) {
			clear();

			srcHex = g;
			srcHexController = srcHex.GetComponent<HexController>();
			srcSelected = true;

			srcHexController.setSelected(true);
		}
		else if (srcHex != null && dstHex == null) {
			dstHex = g;
			dstHexController = dstHex.GetComponent<HexController>();
			dstSelected = true;

			//Debug.Log (src + " - " + dst);

			// action here!
			Coordinate src = new Coordinate(srcHex.GetComponent<ObjectInfo>().line,srcHex.GetComponent<ObjectInfo>().column);
			Coordinate dst = new Coordinate(dstHex.GetComponent<ObjectInfo>().line,dstHex.GetComponent<ObjectInfo>().column);

			//clear();

			if ( gameState.moveStone(src,dst,gameState.getTurn()) )
			{
				refresh = true;

			}

		}

		//Debug.Log ( src + " - " + dst );
	}

	void clear() {

		//Debug.Log (srcSelected + " "+ dstSelected );

		if (srcHex != null) {
			srcHexController.clear();

			srcHex = null;
			srcSelected = false;
		}
		if (dstHex != null) {
			dstHexController.clear();
			
			dstHex = null;
			dstSelected = false;
		}
	}
}
