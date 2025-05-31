using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class SprintUIController : MonoBehaviour
{
    [SerializeField] private Image sprintBarFill;
    [SerializeField] private FirstPersonController player;

    private void Update()
    {
        if (player != null && sprintBarFill != null)
        {
            float normalizedSprint = player.GetSprintNormalized(); // 0.0 to 1.0
            sprintBarFill.fillAmount = normalizedSprint;
            Debug.Log("sprint connected");
        }
    }
}
