using UnityEngine;

public class Data : MonoBehaviour
{
    [SerializeField] private GameObject firstSection;
    [SerializeField] private GameObject secondSection;
    
    public void NextSection()
    {
        firstSection.SetActive(false);
        secondSection.SetActive(true);
    }
}
