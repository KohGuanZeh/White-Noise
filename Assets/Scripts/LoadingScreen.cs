using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen inst;
    [SerializeField] Animator anim;
    [SerializeField] AsyncOperation loading;
    [SerializeField] int currLevel;

    // Start is called before the first frame update
    void Awake() {
        if (inst == null) {
            inst = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        anim = GetComponent<Animator>();
        currLevel = SceneManager.GetActiveScene().buildIndex;
        anim.SetBool("Fade In", false);
    }

    void Update() {
        if (loading != null && loading.isDone) {
            OnLevelLoaded();
        }
    }

    public void LoadNextLevel(int sceneIdx) {
        currLevel = sceneIdx;
        anim.SetBool("Fade In", true);
    }

    void OnScreenFadeIn() {
        if (anim.GetBool("Fade In")) {
            loading = SceneManager.LoadSceneAsync(currLevel, LoadSceneMode.Single);
        }
    }

    void OnLevelLoaded() {
        anim.SetBool("Fade In", false);
        loading = null;
    }
}
