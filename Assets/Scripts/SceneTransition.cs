using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Escena destino")]
    public string sceneName;

    [Header("Condición")]
    public bool requireGateOpen = true;
    public ToriGate parentGate; 

    [Header("Tag requerido")]
    public string playerTag = "Player";

    void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (requireGateOpen && (parentGate == null || !parentGate.IsOpen))
        {
            Debug.Log("El Torii Gate está cerrado, no se puede pasar.");
            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneTransition: no se asignó un nombre de escena.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
