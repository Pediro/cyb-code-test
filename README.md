## An Introduction

The web app is sitting in [angular-cyb-code-test](https://github.com/Pediro/cyb-code-test/tree/master/angular-cyb-code-test) and we have some tests in [cyb-code-test-tests](https://github.com/Pediro/cyb-code-test/tree/master/cyb-code-test-tests). Tests were implemented as needed, mainly to provide a simple connection point to check the implemented logic was running as expected, this avoided the need for me to run the entire web app.

The project was built with ASP.NET Core with an Angular2 front-end, you can find the front-end app in the folder named "ClientApp".

I had issues with the Disney API due to 503 errors, potentially due to the number of requests I was firing at them. To combat this I ended up downloading the entire character dataset and sorting it as a JSON file in the project, which is why the project contains that data folder. The data was then implemented as though I was polling data from a database. I had a high number of requests sent to the Disney API as I was trying to reduce the amount of data I polled each time, as I didn't want to return a large list of data and loop through them for performance reasons. This with the fact I was firing multiple requests for data asynchronously would have potentially helped improve load times when generating the questions and checking results. Maybe a bit overkill but it's how I would have implemented it in a real scenario.

While we're on the topic of the Disney API, I found that some of the characters did not have an image. So when retrieving data we do a get request on the given image url to check if it returns 404, if it does we skip that character. Also, if they don't have any films or TV shows I skip them.

## Technologies used

As mentioned earlier we built the project in ASP.NET Core with Angular2.

Angular2s format of components to handle rendering and logic allows us to decouple the app into manageable bits. This also lets us conform to SOLID principles and the ability to reuse components will mean we pass the DRY test.

Similarly in the backend, I have utilised dependency injection (DI) to inject objects where required to decouple classes from each other, which is fortified with our use of interfaces when defining DI. This has the extra benefit of allowing us to switch the implementation with other classes that inherit the same interface.

Initially, I did write the front-end in vanilla javascript, HTML and CSS but it was a pretty messy file of code. Having the entire set of logic in one file with everything relying on each other could have caused issues down the line if I wanted to change one part of the web app. So the project was re-written in Angular2.

I also stole a spinner from this page: https://loading.io/css/

## Design patterns

A service pattern was used to keep logic in each class small and have single responsibilities. Services generally contain small tasks, such as polling or updating data. Operations will use these services to complete more complex tasks, for example validating given answers and generating the questions for the game. These small services and operations mean testing is easy as there is never a big method to run through.

I'd also like to bring focus to the middleware created for handling exceptions. To prevent endpoints in the app from returning sensitive data to the caller I have set up some middleware that catches any given exception, logs it for our needs, and sends only necessary data to the caller.

