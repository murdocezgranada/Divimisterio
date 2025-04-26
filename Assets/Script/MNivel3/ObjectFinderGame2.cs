using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ObjectFinderGame2 : MonoBehaviour
{
    [System.Serializable]
    public class ObjectQuizData
    {
        public GameObject objectToFind;
        public GameObject quizPanel;
        public Image questionImage;
        public Button[] correctButtons;
        public Button[] wrongButtons;
        public GameObject winImage;
        public GameObject tryAgainImage;
        public Button continueButton;
        public int requiredCorrectAnswers = 2; //  NUEVO: cantidad de respuestas correctas necesarias
    }

    public List<ObjectQuizData> objectQuizzes;
    public AudioSource audioSource;
    public AudioClip questionClip;
    public AudioClip endClip;
    public AudioClip correctAnswerClip;
    public AudioClip wrongAnswerClip;

    public AudioSource backgroundMusic;
    public GameObject finalPanel;
    public Button nextLevelButton;

    private ObjectQuizData currentQuiz;
    private Dictionary<ObjectQuizData, int> correctAnswersGiven = new Dictionary<ObjectQuizData, int>();

    void Start()
    {
        foreach (var quiz in objectQuizzes)
        {
            quiz.quizPanel.SetActive(false);
            quiz.winImage.SetActive(false);
            quiz.tryAgainImage.SetActive(false);
            quiz.continueButton.gameObject.SetActive(false);
            quiz.continueButton.onClick.AddListener(() => ContinueAfterWin(quiz));
            SetupButtons(quiz);
        }

        if (finalPanel != null)
        {
            finalPanel.SetActive(false);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(LoadNextLevel);
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    void SetupButtons(ObjectQuizData quiz)
    {
        correctAnswersGiven[quiz] = 0;

        foreach (Button btn in quiz.correctButtons)
        {
            btn.onClick.AddListener(() => CorrectAnswer(quiz, btn));
        }

        foreach (Button btn in quiz.wrongButtons)
        {
            btn.onClick.AddListener(() => WrongAnswer(quiz));
        }
    }

    public void ObjectClicked(GameObject obj)
    {
        currentQuiz = objectQuizzes.Find(q => q.objectToFind == obj);

        if (currentQuiz != null)
        {
            obj.SetActive(false);

            if (backgroundMusic != null && backgroundMusic.isPlaying)
            {
                backgroundMusic.Pause();
            }

            ShowQuiz(currentQuiz);
        }
    }

    void ShowQuiz(ObjectQuizData quiz)
    {
        quiz.quizPanel.SetActive(true);
        quiz.winImage.SetActive(false);
        quiz.tryAgainImage.SetActive(false);
        quiz.continueButton.gameObject.SetActive(false);
        ShowQuestionElements(quiz, true);
        PlayQuestionSound();
    }

    void PlayQuestionSound()
    {
        if (audioSource != null && questionClip != null)
        {
            audioSource.PlayOneShot(questionClip);
            StartCoroutine(WaitForQuestionEnd());
        }
    }

    IEnumerator WaitForQuestionEnd()
    {
        yield return new WaitForSeconds(56f);

        if (currentQuiz != null && currentQuiz.quizPanel.activeSelf)
        {
            ShowQuestionElements(currentQuiz, false);
            currentQuiz.tryAgainImage.SetActive(true);

            if (audioSource != null && wrongAnswerClip != null)
            {
                audioSource.PlayOneShot(wrongAnswerClip);
            }

            yield return new WaitForSeconds(2f);

            currentQuiz.tryAgainImage.SetActive(false);
            ShowQuestionElements(currentQuiz, true);
            PlayQuestionSound();
        }
    }

    void PlayEndSound()
    {
        if (audioSource != null && endClip != null)
        {
            audioSource.PlayOneShot(endClip);
        }
    }

    void CorrectAnswer(ObjectQuizData quiz, Button clickedButton)
    {
        clickedButton.interactable = false;
        correctAnswersGiven[quiz]++;

     if (correctAnswersGiven[quiz] >= quiz.requiredCorrectAnswers) //  Aquí se usa la cantidad definida
        {
            StopAllCoroutines();

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (correctAnswerClip != null)
            {
                audioSource.PlayOneShot(correctAnswerClip);
            }

            ShowQuestionElements(quiz, false);
            quiz.winImage.SetActive(true);
            quiz.continueButton.gameObject.SetActive(true);
        }
    }

    void ContinueAfterWin(ObjectQuizData quiz)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }

        quiz.winImage.SetActive(false);
        quiz.continueButton.gameObject.SetActive(false);
        quiz.quizPanel.SetActive(false);
        objectQuizzes.Remove(quiz);
        CheckGameCompletion();
    }

    void WrongAnswer(ObjectQuizData quiz)
    {
        StopAllCoroutines();

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (wrongAnswerClip != null)
        {
            audioSource.PlayOneShot(wrongAnswerClip);
        }

        ShowQuestionElements(quiz, false);
        quiz.tryAgainImage.SetActive(true);
        StartCoroutine(ResetQuizPanel(quiz));
    }

    IEnumerator ResetQuizPanel(ObjectQuizData quiz)
    {
        yield return new WaitForSeconds(2f);
        quiz.tryAgainImage.SetActive(false);
        correctAnswersGiven[quiz] = 0;
        ShowQuestionElements(quiz, true);
        PlayQuestionSound();
    }

    void ShowQuestionElements(ObjectQuizData quiz, bool show)
    {
        quiz.questionImage.gameObject.SetActive(show);

        foreach (Button btn in quiz.correctButtons)
        {
            btn.gameObject.SetActive(show);
            btn.interactable = true;
        }

        foreach (Button btn in quiz.wrongButtons)
        {
            btn.gameObject.SetActive(show);
        }
    }

    void CheckGameCompletion()
    {
        if (objectQuizzes.Count == 0)
        {
            Debug.Log("¡Todos los objetos han sido encontrados!");

            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
            }

            if (finalPanel != null)
            {
                finalPanel.SetActive(true);
            }
        }
    }

    void LoadNextLevel()
    {
        // SceneManager.LoadScene("NombreDeTuSiguienteEscena");
    }
}
