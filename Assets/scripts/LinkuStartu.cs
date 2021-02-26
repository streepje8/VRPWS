using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Valve.VR;

public class LinkuStartu : MonoBehaviour
{

    public bool started = false;
    public GameObject blocks;
    public AudioSource startu;

    private AsyncOperation asyncOperation;
    private bool isloading = false;

    IEnumerator LoadScene()
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            blocks.transform.position = new Vector3(3.19f, -8, 270);
            GetComponent<Camera>().backgroundColor = Color.black;
            started = true;
            GetComponent<PlayableDirector>().time = 0f;
            GetComponent<PlayableDirector>().Play();
            startu.time = 0;
            startu.Play();
        }
        if(started)
        {
            GetComponent<Camera>().backgroundColor = Color.Lerp(GetComponent<Camera>().backgroundColor, new Color(0.8f, 0.8f, 0.8f), 1f * Time.deltaTime);
        }
        if((!startu.isPlaying) && started)
        {
            SteamVR_Fade.Start(new Color(255, 255, 255), 0.1f);
            if (!isloading)
            {
                StartCoroutine(LoadScene());
                isloading = true;
            }
        }
    }
}
