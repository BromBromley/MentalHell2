using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    
    void Start()
    {
        root = uiDocument.rootVisualElement;
    }
}