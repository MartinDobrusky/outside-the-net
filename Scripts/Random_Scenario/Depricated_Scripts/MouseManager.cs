// using UnityEngine;
//
// public class MouseManager : MonoBehaviour
// {
//     private Camera _camera;
//     
//     private void Start()
//     {
//         _camera = Camera.main;    
//     }
//
//     void Update()
//     {
//         if (!Input.GetMouseButtonDown(0)) return;
//         var ray = _camera.ScreenPointToRay(Input.mousePosition);
//         if (Physics.Raycast(ray, out var hit)){
//             if (hit.rigidbody != null)
//             {
//                 hit.rigidbody.AddExplosionForce(5000, hit.point, 10);
//             }
//         }
//     }
// }
