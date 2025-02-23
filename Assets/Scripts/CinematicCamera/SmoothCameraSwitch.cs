using System.Collections;
using Observers;
using Observers.dto;
using StateMachine;
using StateMachine.states;
using UnityEngine;

namespace CinematicCamera
{
    public class SmoothCameraSwitch : Observable<MainMenuDTO>
    {
        public Camera[] cameras;
        public GameObject menuCanvas;
        public GameObject tiltBarCanvas;
        public GameObject gameCanvas;
        public float transitionTime = 1.0f;
        public static int currentCameraIndex = 0;
        public KeyCode switchKey = KeyCode.J;
        private bool _isSwitching = false;
        private bool _hasSwitched = false; 

        private Vector3[] originalPositions;
        private Quaternion[] originalRotations;

        void Start()
        {
            originalPositions = new Vector3[cameras.Length];
            originalRotations = new Quaternion[cameras.Length];
            for (int i = 0; i < cameras.Length; i++)
            {
                originalPositions[i] = cameras[i].transform.position;
                originalRotations[i] = cameras[i].transform.rotation;
                cameras[i].enabled = (i == currentCameraIndex);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(switchKey) && !_isSwitching && !_hasSwitched && StateMachineManager.instance.story ==true)
            {
                int nextCameraIndex = (currentCameraIndex + 1) % cameras.Length;
                StartCoroutine(SwitchCamera(nextCameraIndex));
                _hasSwitched = true;
                menuCanvas.SetActive(false);
                tiltBarCanvas.SetActive(true);
                gameCanvas.SetActive(true);


                if (StateMachineManager.instance.GetCurrentState() is MainMenuState)
                {
                    NotifyObservers(new MainMenuDTO());
                    StateMachineManager.instance.StartState();
                }
            }
        }

        IEnumerator SwitchCamera(int newIndex)
        {
            _isSwitching = true;
            Camera currentCamera = cameras[currentCameraIndex];
            Camera nextCamera = cameras[newIndex];

            nextCamera.transform.position = originalPositions[newIndex];
            nextCamera.transform.rotation = originalRotations[newIndex];
            nextCamera.enabled = true;

            float elapsedTime = 0;
            while (elapsedTime < transitionTime)
            {
                float t = elapsedTime / transitionTime;
                nextCamera.transform.position = Vector3.Lerp(currentCamera.transform.position, originalPositions[newIndex], t);
                nextCamera.transform.rotation = Quaternion.Lerp(currentCamera.transform.rotation, originalRotations[newIndex], t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            nextCamera.transform.position = originalPositions[newIndex];
            nextCamera.transform.rotation = originalRotations[newIndex];

            currentCamera.enabled = false;
            currentCameraIndex = newIndex;
            _isSwitching = false;
        }
    }
}
