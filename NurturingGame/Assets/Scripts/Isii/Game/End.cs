using System.Threading.Tasks;
using UnityEngine;

public class End : MonoBehaviour
{
    public static End Instance { get; private set; }

    [SerializeField] private GameObject endUI;
    [SerializeField] private GameObject resultImage;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        ResultEnd();
    }




    void ResultEnd()
    {
        endUI.SetActive(true);
        resultImage.SetActive(true);
        WaitResultEnd();
    }





    async void WaitResultEnd()
    {
        await Task.Delay(3000);
        resultImage.SetActive(false);
    }







    public void PushTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestTitle");
    }















}
