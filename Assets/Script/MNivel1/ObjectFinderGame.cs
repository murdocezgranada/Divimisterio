using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ObjectFinderGame : MonoBehaviour
{
    [System.Serializable]
    public class ObjectQuizData
    {
        public GameObject objectToFind;
        public GameObject quizPanel;
        public Image questionImage;
        public Button correctButton;
        public Button[] wrongButtons;
        public GameObject winImage;
        public GameObject tryAgainImage;
        public Button continueButton;
    }

    public List<ObjectQuizData> objectQuizzes;
    public AudioSource audioSource;
    public AudioClip questionClip;
    public AudioClip endClip;
    public AudioClip correctAnswerClip;
    public AudioClip wrongAnswerClip;  // Sonido de "perdiste"

    public AudioSource backgroundMusic;
    public GameObject finalPanel;
    public Button nextLevelButton;

    private ObjectQuizData currentQuiz;

    void Start()
    {
        foreach (var quiz in objectQuizzes)
        {
            quiz.quizPanel.SetActive(false);
            quiz.winImage.SetActive(false);
            quiz.tryAgainImage.SetActive(false);
            quiz.continueButton.gameObject.SetActive(false);
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
        quiz.correctButton.onClick.AddListener(() => CorrectAnswer(quiz));
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
            // Ocultamos pregunta y botones
            ShowQuestionElements(currentQuiz, false);

            // Mostramos mensaje de error
            currentQuiz.tryAgainImage.SetActive(true);

            // Reproducimos el sonido de "perdiste"
            if (audioSource != null && wrongAnswerClip != null)
            {
                audioSource.PlayOneShot(wrongAnswerClip); // Sonido de "perdiste"
            }

            yield return new WaitForSeconds(2f);

            // Ocultamos mensaje de error y volvemos a mostrar la pregunta
            currentQuiz.tryAgainImage.SetActive(false);
            ShowQuestionElements(currentQuiz, true);
            PlayQuestionSound(); // Repetimos la pregunta
        }
    }

    void CorrectAnswer(ObjectQuizData quiz)
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

        quiz.continueButton.onClick.RemoveAllListeners();
        quiz.continueButton.onClick.AddListener(() => OnContinueClicked(quiz));
    }

    void OnContinueClicked(ObjectQuizData quiz)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }

        quiz.winImage.SetActive(false);
        quiz.quizPanel.SetActive(false);
        quiz.continueButton.gameObject.SetActive(false);
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
        StartCoroutine(ResetQuizPanel(quiz));
    }

    IEnumerator ResetQuizPanel(ObjectQuizData quiz)
    {
        quiz.tryAgainImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        quiz.tryAgainImage.SetActive(false);
        ShowQuestionElements(quiz, true);
        PlayQuestionSound();
    }

    void ShowQuestionElements(ObjectQuizData quiz, bool show)
    {
        quiz.questionImage.gameObject.SetActive(show);
        quiz.correctButton.gameObject.SetActive(show);
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

