using UnityEngine;

namespace Assets.Scripts.Orig {
public class Nav : MonoBehaviour {

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }
}
}
