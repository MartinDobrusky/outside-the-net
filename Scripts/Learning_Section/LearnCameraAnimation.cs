using UnityEngine;
using UnityEngine.UI;

public class LearnCameraAnimation : MonoBehaviour {

    // An array of positions that trigger animations
    public float[] animationPositions;

    // An array of camera animations to play
    public Animator cameraAnimator;

    // A reference to the scroll view component
    public ScrollRect scrollView;

    // A reference to the main camera
    public Camera mainCamera;

    // The index of the currently active animation
    private int activeAnimationIndex = 0;

    public void OnScroll(Vector2 scrollPosition)
    {
        if (scrollPosition.y < animationPositions[activeAnimationIndex]) {
            activeAnimationIndex++;
            cameraAnimator.SetInteger("Stage", activeAnimationIndex);
        }
    }
}