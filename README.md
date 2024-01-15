# cyb-code-test

## Libraries and Technologies used

The app has been built in C# Asp.Net Core with a vanilla javascript, html, and css front end. 

Front-end frameworks such as React or Angular were considered at the start but I didn't necessarily need a lot of the features they provide. After sitting down and planning the design of the app, it was clear that the work could be done with just javascript and I kept with it.

In terms of deciding on a back-end, it was purely opinion-based as I knew I could program the server logic quickly in C#. 

Honestly, decisions on the tech stack were all opinion-based. I picked the ones I felt most comfortable with but if we required modular elements over many pages I would have gone for React. In theory, the rows in the submit and results page are good candidates for modules rather than the for loops I created. Similarly, if we wanted to move the back-end logic to serverless AWS lambda then I would have picked nodejs or python due to their shorter warm-up times.

A stretch goal of mine is to port them to svelte and nodejs as they are in the CYB tech stack.

My libraries include:
- Newtonsoft.Json, which is the industry accepted package for handing JSON in C#
- NUnit, a testing package

I also stole a spinner from this page: https://loading.io/css/

Tests were implemented as needed, mainly to provide a simple connection point to check the implemented logic was running as expected, this avoided the need for me to run the entire web app.

## Design patterns and Tech notes

I had issues with the Disney API due to 503 errors I kept receiving, potentially due to the number of requests I was firing at them. To combat this I ended up downloading the entire character dataset and sorting it as a JSON file in the project, you will find it under the cyb-code-test/Data folder. The data was then implemented as though I was polling data from a database. I had a high number of requests sent to the Disney API as I was trying to reduce the amount of data I polled each time, as I didn't want to return a large list of data and loop through them for performance reasons. This with the fact I was firing multiple requests for data asynchronously would have potentially helped improve load times when generating the questions and checking results. Maybe a bit overkill but it's how I would have implemented it in a real scenario. 

The app was designed with a separate front-end and REST API back-end. This choice allows me to keep the front-end active, for rendering loading animations or other engagement elements, while I process data in the back.
The backend uses dependency injection to inject objects where required to decouple classes from each other. I use a service pattern to keep logic in each class small and have single responsibilities, basically I'm trying to conform to DRY and SOLID principles. Services generally contain small tasks, such as polling or updating data. Operations will use these services to complete more complex tasks, for example validating given answers and generating the questions for the game. These small services and operations mean testing is easy as there is never a big method to run through.

I'd like to bring focus to the middleware created for handling exceptions. To prevent endpoints in the app from returning sensitive data to the caller I have set up some middleware that catches any given exception, logs it for our needs, and sends only necessary data to the caller.
