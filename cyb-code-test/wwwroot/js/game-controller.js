class GameController {

    questions = {};
    currentQuestion = {};
    currentQuestionIndex = 0;

    constructor() { }

    init() {
        var httpRequest = new XMLHttpRequest();

        httpRequest.onreadystatechange = function () {
            if (httpRequest.readyState == XMLHttpRequest.DONE) {
                if (httpRequest.status !== 200) {
                    location.href = "/guessTheCharacter/error";
                }

                var response = httpRequest.responseText;
                this.questions = JSON.parse(response);

                this.initQuestion();
                this.addEventListeners();
                this.renderQuestionPage();
            }
        }.bind(this);

        httpRequest.open("get", "/guessTheCharacter/fetchGameData", true);
        httpRequest.send();
    }

    addEventListeners() {
        var answerOptions = document.querySelectorAll("[data-set-answer]");
        answerOptions.forEach((element, index) => {
            element.addEventListener("click", () => {
                this.questions[this.currentQuestionIndex].selectedAnswer = element.textContent;
                this.goToNextQuestion();
            });
        });
    }

    initQuestion() {
        this.currentQuestion = this.questions[this.currentQuestionIndex];

        var pageTitleElement = document.querySelector("[data-set-page-title]");
        pageTitleElement.textContent = "Which film or tv show has " + this.currentQuestion.characterName + " been in?";

        var imageElement = document.querySelector("[data-set-question-image]");
        imageElement.src = this.currentQuestion.imageUrl;

        var answerOptions = document.querySelectorAll("[data-set-answer]");
        answerOptions.forEach((element, index) => {
            element.textContent = this.currentQuestion.answers[index];
        });
    }

    renderQuestionPage() {
        var questionsPage = document.querySelector("[data-set-questions-page]");
        questionsPage.classList.remove("hidden");

        var submitPage = document.querySelector("[data-set-submit-page]");
        submitPage.classList.add("hidden");

        var resultsPage = document.querySelector("[data-set-results-page]");
        resultsPage.classList.add("hidden");
    }

    goToNextQuestion() {
        this.currentQuestionIndex++;

        if (this.currentQuestionIndex > 9) {
            this.goToSubmitScreen();
        } else {
            this.initQuestion();
        }
    }

    goToPreviousQuestion() {
        this.currentQuestionIndex--;

        if (this.currentQuestionIndex < 0) {
            this.currentQuestionIndex = 0;
        }

        this.initQuestion();
    }

    submitAnswers() {
        var httpRequest = new XMLHttpRequest();

        httpRequest.onreadystatechange = function () {
            if (httpRequest.readyState == XMLHttpRequest.DONE) {
                if (httpRequest.status !== 200) {
                    location.href = "/guessTheCharacter/error";
                }

                var response = httpRequest.responseText;
                var results = JSON.parse(response);
                this.goToResultsPage(results);
            }
        }.bind(this);

        httpRequest.open("post", "/guessTheCharacter/submitAnswers", true);
        httpRequest.setRequestHeader('Content-Type', 'application/json');

        httpRequest.send(JSON.stringify(this.questions));
    }

    goToSubmitScreen() {
        var questionsPage = document.querySelector("[data-set-questions-page]");
        questionsPage.classList.add("hidden");

        var submitPage = document.querySelector("[data-set-submit-page]");
        submitPage.classList.remove("hidden");

        var resultsPage = document.querySelector("[data-set-results-page]");
        resultsPage.classList.add("hidden");

        var answersContainer = document.querySelector("[data-set-answers-container]");
        var navButton = document.querySelector("[data-set-nav-button]");
        var firstMissingAnswerIndex = null;
        this.questions.forEach((question, index) => {
            if (question.selectedAnswer === null && firstMissingAnswerIndex === null) {
                firstMissingAnswerIndex = index;
            }

            var answer = document.createElement("div");
            answer.classList.add("answer");

            var img = document.createElement("img");
            img.src = question.imageUrl;
            answer.appendChild(img);

            var selectedAnswer = document.createElement("text");
            selectedAnswer.textContent = question.selectedAnswer ?? "";
            answer.appendChild(selectedAnswer);

            answersContainer.append(answer);
        });

        var pageTitleElement = document.querySelector("[data-set-page-title]");
        if (firstMissingAnswerIndex == null) {
            pageTitleElement.textContent = "Ready to submit your answers?";
            navButton.classList.add("hidden");
        } else {
            pageTitleElement.textContent = "You've not answered all the questions."
            navButton.textContent = "Go to unanswered";
            navButton.classList.remove("hidden");
        }

        answersContainer.append();

        var submitButton = document.querySelector("[data-set-submit-button]");
        submitButton.addEventListener("click", this.submitAnswers.bind(this));
    }

    goToResultsPage(results) {
        var questionsPage = document.querySelector("[data-set-questions-page]");
        questionsPage.classList.add("hidden");

        var submitPage = document.querySelector("[data-set-submit-page]");
        submitPage.classList.add("hidden");

        var resultsPage = document.querySelector("[data-set-results-page]");
        resultsPage.classList.remove("hidden");

        var resultsContainer = document.querySelector("[data-set-results-container]");

        var correctCount = 0;
        results.forEach((result) => {
            var resultElement = document.createElement("div");
            resultElement.classList.add("answer");

            if (result.isCorrectAnswer) {
                resultElement.classList.add("correct");
                correctCount++;
            } else {
                resultElement.classList.add("incorrect");
            }

            var img = document.createElement("img");
            img.src = result.imageUrl;
            resultElement.appendChild(img);

            var selectedAnswer = document.createElement("text");
            selectedAnswer.textContent = result.selectedAnswer ?? "";
            resultElement.appendChild(selectedAnswer);

            resultsContainer.append(resultElement);
        });

        var pageTitleElement = document.querySelector("[data-set-page-title]");
        pageTitleElement.textContent = `Well done! You got ${correctCount} of the questions correct.`;

        var resetButton = document.querySelector("[data-set-reset-button]");
        resetButton.addEventListener("click", () => { location.reload(); });
    }
}