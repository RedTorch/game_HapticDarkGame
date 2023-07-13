using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSwitcherManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoadOnInput;
    [SerializeField] private GameObject canvasObject;
    private float alpha = 1f;
    private bool isFadeOut = false;
    [SerializeField] private float fadeSpeedMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && alpha <= 0f)
        {
            isFadeOut = true;
        }

        if(isFadeOut)
        {
            alpha += Time.deltaTime * fadeSpeedMultiplier;
        }
        else
        {
            alpha -= Time.deltaTime * fadeSpeedMultiplier;
        }
        alpha = Mathf.Clamp(alpha,0f,1f);
        canvasObject.GetComponent<CanvasGroup>().alpha = alpha;
        
        if(alpha == 1f && isFadeOut)
        {
            SceneManager.LoadScene(sceneToLoadOnInput);
            isFadeOut = false;
        }
    }
}
