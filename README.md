# cyb-code-test

## Libraries and Technologies used

The app has been built in C# Asp.Net Core with a vanilla javscript, html and css front end.
These choices where purely opinion based as these are the technologies I can program the quickest in. A stretch goal of mine is to port them to svelte and node js as they are in the CYB tech stack.

My libraries include:
- Newtonsoft.Json, which is the industry accepted package for handing json in C#
- NUnit, a testing package

I noticed that you set tests as a stretch goal but I implemented them near the start. Mainly because they provided me with a simple connection point to check the logic I was working on was working as expected, avoiding a need for me to produce some interface to check against. They are not perfect tests but they greatly sped up my implementation.

## Design patterns and Tech notes

I had issues with the Disney API due to 503 errors I kept recieving, potentially due to the number of requests I was firing at them. To combat this I ended up downloading the entire character dataset and sorting it as a json file in the project, you will find it under the Data folder. I had a high number of requests sending to them as I was trying to reduce the amount of data I polled each time. This with the fact I was firing multiple requests for data asynchrously would have potentially helped improve load times when generating the questions and checking results. Maybe a bit overkill and it made less sense once I had the data locally so I removed the logic. 

The app was designed with a separate front-end and REST API back-end. This choice allows me to keep the front-end active, for rendering loading animations or other engagement elements, while I process data in the back.
The backend uses dependency injection to inject objects where required in an effort to decouple classes from each other. I use a service pattern to keep logic in each class small and have single responsibilities, basically I'm trying to conform to DRY and SOLID principles. Services generally contain small tasks, such as polling or updating data. And operations will use these services to complete more complex tasks, for example validating given answers and generating the questions for the game.

I'd like to bring focus to the middleware I created for handling exceptions. To prevent elements in the app returning sensitive data to the caller I have set up some middleware that catches any given exception, logs it for our needs, and sends only necessary data to the caller.
