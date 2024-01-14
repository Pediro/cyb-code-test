# cyb-code-test

## Libraries and Technologies used

The app has been built in C# Asp.Net Core with a vanilla javscript, html and css front end.
These choices where purely opinion based as these are the technologies I can program the quickest in.

My libraries include:
- Newtonsoft.Json, which is the industry accepted package for handing json in C#
- NUnit, a testing package

Tests were a stretch goal, but they provided me a simple connection point to check the logic I was programming would work as I expect them to in the app.

## Design patterns and Tech notes

It should be noted that due to a 503 error I was facing with the Disney API, I downloaded the entire characters dataset and stored it as a json file in the project. I believe this might have been due to my high request rate, I was aiming to reduce the amount of data I poll to improve performance on my web server and run them asynchroously so all questions can be generated at the same time.

The app was designed with a separate front-end and REST API back-end. This choice keeps the front end loading while I process data in the back, I can rendering loading animations or other engaging elements to keep the user from viewing a white loading screen.
The backend uses dependency injection to inject objects where required in an effort to decouple classes from each other. Injected objects are split into services and operations. Services hold smaller bits of logic, such as polling data, and the operations use these services to complete more complex tasks.

I'd like to bring focus to the middleware used for handling exceptions. To prevent elements in the app returning sensitive data to the caller I have set up some middleware that catches any given exception, logs it for our needs, and sends only necessary data to the caller.
