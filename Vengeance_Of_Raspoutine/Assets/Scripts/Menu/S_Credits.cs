using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Credits : MonoBehaviour
{
    [SerializeField] private Animator m_animatorCredits;
    public string m_AnimationName;

    // Update is called once per frame
    void Update()
    {

        AnimatorStateInfo stateInfo = m_animatorCredits.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(m_AnimationName))
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
