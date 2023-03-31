// using System;
// using System.Collections;
//
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
// public class LazyLoadGame : MonoBehaviour
// {
//     [SerializeField] private string _sceneName = "RandomScenarioMap";
//     [SerializeField] private Button _playButton;
//     public string _SceneName => this._sceneName;
//
//     private AsyncOperation _asyncOperation;
//
//     private void Start()
//     {
//         Debug.Log("Started Scene Preloading");
//         
//         this.StartCoroutine(this.LoadSceneAsyncProcess(sceneName: this._sceneName));
//     }
//
//     private IEnumerator LoadSceneAsyncProcess(string sceneName)
//     {
//         this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);
//         
//         this._asyncOperation.allowSceneActivation = false;
//
//         while (!this._asyncOperation.isDone)
//         {
//             Debug.Log($"[scene]:{sceneName} [load progress]: {this._asyncOperation.progress}");
//             
//             this._playButton.interactable = true;
//
//             yield return null;
//         }
//     }
//
//     public void StartGame()
//     {
//         if (this._asyncOperation != null)
//         {
//             Debug.Log("Allowed Scene Activation");
//             
//             this._asyncOperation.allowSceneActivation = true;
//         }
//     }
// }
