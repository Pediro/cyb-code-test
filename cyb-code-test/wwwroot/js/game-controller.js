class GameController {

    questions = {};
    currentQuestion = {};
    currentQuestionIndex = 0;
    isLoading = false;

    headerElement;
    answerOptions;
    pageTitleElement;
    imageElement;
    landingPage;
    questionsPage;
    submitPage;
    resultsPage;
    answersContainer;
    navButton;
    pageTitleElement;
    submitButton;
    resultsContainer;
    resetButton;

    constructor() {
        this.headerElement = document.querySelector("[data-set-header-element]");
        this.answerOptions = document.querySelectorAll("[data-set-answer]");
        this.pageTitleElement = document.querySelector("[data-set-page-title]");
        this.imageElement = document.querySelector("[data-set-question-image]");
        this.landingPage = document.querySelector("[data-set-landing-page]");
        this.questionsPage = document.querySelector("[data-set-questions-page]");
        this.submitPage = document.querySelector("[data-set-submit-page]");
        this.resultsPage = document.querySelector("[data-set-results-page]");
        this.answersContainer = document.querySelector("[data-set-answers-container]");
        this.navButton = document.querySelector("[data-set-nav-button]");
        this.pageTitleElement = document.querySelector("[data-set-page-title]");
        this.submitButton = document.querySelector("[data-set-submit-button]");
        this.resultsContainer = document.querySelector("[data-set-results-container]");
        this.resetButton = document.querySelector("[data-set-reset-button]");

        this.hideAllPages();
    }

    init() {
        this.startLoading();

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
                this.endLoading();
                this.showQuestionPage();
            }
        }.bind(this);

        httpRequest.open("get", "/guessTheCharacter/fetchGameData", true);
        httpRequest.send();
    }

    addEventListeners() {
        this.answerOptions.forEach((element, index) => {
            element.addEventListener("click", () => {
                this.questions[this.currentQuestionIndex].selectedAnswer = element.textContent;
                this.goToNextQuestion();
            });
        });
    }

    initQuestion() {
        this.currentQuestion = this.questions[this.currentQuestionIndex];

        this.pageTitleElement.textContent = "Which film or tv show has " + this.currentQuestion.characterName + " been in?";
        this.imageElement.src = this.currentQuestion.imageUrl;

        this.answerOptions.forEach((element, index) => {
            element.textContent = this.currentQuestion.answers[index];
        });
    }

    showQuestionPage() {
        this.questionsPage.classList.remove("hidden");
        this.submitPage.classList.add("hidden");
        this.resultsPage.classList.add("hidden");
    }

    goToNextQuestion() {
        this.currentQuestionIndex++;

        if (this.currentQuestionIndex > this.questions.length - 1) {
            this.goToSubmitScreen();
            return;
        }

        // Only go to questions that have not been answered, this allows us to do a skip function on the submit page
        if (this.questions[this.currentQuestionIndex].selectedAnswer === null) {
            this.initQuestion();
            return;
        }

        this.goToNextQuestion();
    }

    goToPreviousQuestion() {
        this.currentQuestionIndex--;

        if (this.currentQuestionIndex < 0) {
            this.currentQuestionIndex = 0;
        }

        this.initQuestion();
    }

    goToQuestion(event) {
        this.currentQuestionIndex = parseInt(event.currentTarget.dataset.index);
        this.initQuestion();
        this.showQuestionPage();
    }

    submitAnswers() {
        this.startLoading();
        var httpRequest = new XMLHttpRequest();

        httpRequest.onreadystatechange = function () {
            if (httpRequest.readyState == XMLHttpRequest.DONE) {
                if (httpRequest.status !== 200) {
                    location.href = "/guessTheCharacter/error";
                }

                var response = httpRequest.responseText;
                var results = JSON.parse(response);
                this.endLoading();
                this.goToResultsPage(results);
            }
        }.bind(this);

        httpRequest.open("post", "/guessTheCharacter/submitAnswers", true);
        httpRequest.setRequestHeader('Content-Type', 'application/json');

        httpRequest.send(JSON.stringify(this.questions));
    }

    goToSubmitScreen() {
        this.questionsPage.classList.add("hidden");
        this.submitPage.classList.remove("hidden");
        this.resultsPage.classList.add("hidden");
        var firstMissingAnswerIndex = null;
        this.answersContainer.innerHTML = "";

        this.questions.forEach((question, index) => {
            if (question.selectedAnswer === null && firstMissingAnswerIndex === null) {
                firstMissingAnswerIndex = index;
            }

            var answer = document.createElement("div");
            answer.classList.add("item");
            answer.classList.add("answer");
            answer.dataset.index = index;

            var img = document.createElement("img");
            img.src = question.imageUrl;
            answer.appendChild(img);

            var selectedAnswer = document.createElement("text");
            selectedAnswer.textContent = question.selectedAnswer ?? "";
            answer.appendChild(selectedAnswer);

            answer.addEventListener("click", this.goToQuestion.bind(this));
            this.answersContainer.append(answer);
        });

        if (firstMissingAnswerIndex == null) {
            this.pageTitleElement.textContent = "Ready to submit your answers?";
            this.navButton.classList.add("hidden");
        } else {
            this.pageTitleElement.textContent = "You've not answered all the questions."
            this.navButton.textContent = "Go to unanswered";
            this.navButton.classList.remove("hidden");
        }

        this.answersContainer.append();
        this.submitButton.addEventListener("click", this.submitAnswers.bind(this));
    }

    goToResultsPage(results) {
        this.questionsPage.classList.add("hidden");
        this.submitPage.classList.add("hidden");
        this.resultsPage.classList.remove("hidden");

        var correctCount = 0;
        this.resultsContainer.innerHTML = "";

        results.forEach((result) => {
            var resultElement = document.createElement("div");
            resultElement.classList.add("item");
            resultElement.classList.add("result");

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

            this.resultsContainer.append(resultElement);
        });

        this.pageTitleElement.textContent = `You got ${correctCount} of the questions correct.`;

        this.resetButton.addEventListener("click", () => { location.reload(); });
    }

    startLoading() {
        this.isLoading = true;

        this.headerElement.classList.add("hidden");
        this.questionsPage.classList.add("hidden");
        this.submitPage.classList.add("hidden");
        this.resultsPage.classList.add("hidden");
        this.landingPage.classList.remove("hidden");
    }

    endLoading() {
        this.isLoading = false;
        this.headerElement.classList.remove("hidden");
        this.landingPage.classList.add("hidden");
    }

    hideAllPages() {
        this.headerElement.classList.add("hidden");
        this.questionsPage.classList.add("hidden");
        this.submitPage.classList.add("hidden");
        this.resultsPage.classList.add("hidden");
        this.landingPage.classList.add("hidden");
    }
}