using System.Collections;
using UnityEngine;

public class SmoothCameraSwitch : MonoBehaviour
{
    public Camera[] cameras; // Lista kamer
    public float transitionTime = 1.0f; // Czas przejœcia
    private int currentCameraIndex = 0;
    private bool isSwitching = false;
    private bool hasSwitched = false; // Flaga blokuj¹ca kolejn¹ zmianê

    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;

    void Start()
    {
        // Zapamiêtaj pocz¹tkowe pozycje i rotacje kamer
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
        if (Input.GetKeyDown(KeyCode.J) && !isSwitching) // Prze³¹czanie kamery na spacjê
        {
            int nextCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            StartCoroutine(SwitchCamera(nextCameraIndex));
            hasSwitched = true;
        }
    }

    IEnumerator SwitchCamera(int newIndex)
    {
        isSwitching = true;
        Camera currentCamera = cameras[currentCameraIndex];
        Camera nextCamera = cameras[newIndex];

        // Resetujemy pozycjê i rotacjê kamery docelowej do jej oryginalnych wartoœci
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

        // Ostatecznie ustawiamy dok³adnie oryginaln¹ pozycjê
        nextCamera.transform.position = originalPositions[newIndex];
        nextCamera.transform.rotation = originalRotations[newIndex];

        currentCamera.enabled = false;
        currentCameraIndex = newIndex;
        isSwitching = false;
    }
}
