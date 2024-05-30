using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class S_Credits : MonoBehaviour
{
    [SerializeField] private Animator animatorCredits;
    public string animationName;

    // Update is called once per frame
    void Update()
    {

        AnimatorStateInfo stateInfo = animatorCredits.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animationName))
        {
            // check if the animation is finish
            if (stateInfo.normalizedTime >= 1f)
            {
                // when the animation credits is finish, return automaticaly in the main menu
                SceneManager.LoadScene("Nicolas-Menu");
            }
        }
    }
}
