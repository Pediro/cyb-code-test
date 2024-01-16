# cyb-code-test

## An Introduction

The web app is sitting in [angular-cyb-code-test](https://github.com/Pediro/cyb-code-test/tree/master/angular-cyb-code-test) and we have some tests I created for checking logic as I implemented them in [here](https://github.com/Pediro/cyb-code-test/tree/master/cyb-code-test-tests). Tests were implemented as needed, mainly to provide a simple connection point to check the implemented logic was running as expected, this avoided the need for me to run the entire web app.

The project was designed with a separate front-end and REST API backend. This choice allows me to keep the front-end active, for rendering loading animations or other engagement elements, while I process data in the back.

The front-end app is located in the folder named "ClientApp" which is an Angular2 project, a folder named "Data" which contains the a json file of all the Disney characters and other folders which link to the ASP.NET Core C# back-end.

I had issues with the Disney API due to 503 errors I kept receiving, potentially due to the number of requests I was firing at them. To combat this I ended up downloading the entire character dataset and sorting it as a JSON file in the project, which is why the project contains that data folder. The data was then implemented as though I was polling data from a database. I had a high number of requests sent to the Disney API as I was trying to reduce the amount of data I polled each time, as I didn't want to return a large list of data and loop through them for performance reasons. This with the fact I was firing multiple requests for data asynchronously would have potentially helped improve load times when generating the questions and checking results. Maybe a bit overkill but it's how I would have implemented it in a real scenario.

## Design patterns

Angular2 and ASP.NET Core have some pretty useful features, but the choice to use those frameworks was mainly opinion-based. I knew I could program the logic up quickly and without much issue. But their ability to compartmentise logic into small classes is something really attractive. 

Initially I did write the front-end in vanilla javascript, html and css but it was a pretty messy file of code. Having the entire set of logic in one file with everything relying on each other could have caused issues down the line if I wanted to change one part of the web app. So the project was re-written in Angular2.

Angular2s format of creating components to handling rendering and logic allows us to decouple the app into managable bits.

Similarly in the backend, I have utilised dependency injection (DI) to inject objects where required to decouple classes from each other, which is fortified with our use of interfaces when defining DI. This has the extra benefit of allowing us to switch the implementation with other classes that inherit the same interface.

I use a service pattern to keep logic in each class small and have single responsibilities. Services generally contain small tasks, such as polling or updating data. Operations will use these services to complete more complex tasks, for example validating given answers and generating the questions for the game. These small services and operations mean testing is easy as there is never a big method to run through.

Both the items described above allow us to easily change components in our logic without affecting the entire app, allowing for more comfortable development when making changes.

I'd also like to bring focus to the middleware created for handling exceptions. To prevent endpoints in the app from returning sensitive data to the caller I have set up some middleware that catches any given exception, logs it for our needs, and sends only necessary data to the caller.

## Libraries

There weren't any big libraries I took advantage of for this project. I did utilise the Newtonsoft JSON package for handling JSON in the back-end and NUnit for testing, but these are industry standard. 

In the front-end it was mainly angular related dependencies such as routing and creating promises for asynchronous development.

I think logic wise there wasn't any libraries I could have taken advantage off but I may have been able to use slider css or other bootstrap css libraries to help create more interesting UI. 

I did a spinner from this page: https://loading.io/css/



