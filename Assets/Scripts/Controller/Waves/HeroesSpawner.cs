using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HeroesSpawner : MonoBehaviour {
    public List<Wave> waves;
    public float timeBetweenWages;
    public float timeCountdownStart;

    private int indexWave;
    private GameObject waveInstance;

    public void Awake() {
        Debug.Log("Awake");
        waveInstance = new GameObject("wave instance");
        waveInstance.transform.parent = this.transform;

        indexWave = -1;
    }

    public void Start() {
        //create an instance for the waves to hold needed scripts and other things
       
        StartCoroutine(GameLoop());
    }
    
    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop() {
        bool inGame = true;

        while (inGame) {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundStarting());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine(RoundEnding());


            //if player has died or all heroes has died TODO
            //if (false) {
                //trigger event of game over TODO
            //    inGame = false;
            //}
        }
    }


    private IEnumerator RoundStarting() {
        Debug.Log("Start round");
        ++indexWave;
        
        //show message of new round starting (countdown)
        // Increment the round number and display text showing the players what round it is.
        //m_MessageText.text = "ROUND " + GameState.Instance.RoundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return new WaitForSeconds(timeCountdownStart);
    }


    private IEnumerator RoundPlaying() {
        Debug.Log("Index: "+indexWave);
        waves[indexWave].StartWave(waveInstance);

        // Clear the text from the screen.
        //m_MessageText.text = string.Empty;

        // While the wave is not over or player don't die (todo)
        while (!waves[indexWave].WaveOver()) {
            waves[indexWave].UpdateWave();
            yield return null;
        }
    }


    private IEnumerator RoundEnding() {
        //clear every data of game object for waves
        foreach (var comp in waveInstance.GetComponents<Component>()) {
            if (!(comp is Transform)) {
                Destroy(comp);
            }
        }

        //if player has died just return

        //if not
        // show any message info after a wave
        //string message = EndMessage(winner);
        //m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return new WaitForSeconds(timeBetweenWages);
    }
}
