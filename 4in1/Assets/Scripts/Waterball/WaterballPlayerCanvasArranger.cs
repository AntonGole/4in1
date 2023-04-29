using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterballPlayerCanvasArranger : NetworkBehaviour {

    public GameObject player;

    private int playerID;
    private Canvas canvas; 
    
    
    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        playerID = player.GetComponent<WaterballPlayer>().playerID;
        



    }


    
    
    
}




// private void CheckScenes() {
//     var scene = SceneManager.GetActiveScene().name;
//
//     switch (scene) {
//         case "Title Screen":
//             return;
//         default:
//             return;
//     }
// }


//
// private void DisplayTitleScreenCanvas() {
//     var position = calculateButtonPosition(playerID);
//     var rotation = calculateButtonRotation(playerID);
//
//     var canvas = "hello"; 
//
//
//
// }
//
//
//
//
// private Vector3 calculateButtonPosition(int playerID) {
//     var x = 140;
//     var y = 25;
//     var z = 0;
//
//     switch (playerID) {
//         case 0:
//             return new Vector3(x, -y, z);
//         case 1:
//             return new Vector3(-x, -y, z);
//         case 2:
//             return new Vector3(x, y, z);
//         case 3:
//             return new Vector3(-x, y, z);
//         default:
//             return new Vector3(0, 0, 0);
//
//     }
// }
//
// private Quaternion calculateButtonRotation(int playerID) {
//     switch (playerID) {
//         case 0:
//             return Quaternion.Euler(0, 0, 45);
//         case 1:
//             return Quaternion.Euler(0, 0, -45);
//         case 2:
//             return Quaternion.Euler(0, 0, 135);
//         case 3:
//             return Quaternion.Euler(0, 0, -135);
//         default:
//             return Quaternion.Euler(0, 0, 0);
//
//     }
// }
//
//
//


