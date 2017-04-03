# candle

*Project Description*

This sample is used to promote [dsl tools](http://msdn.microsoft.com/en-us/library/bb126235.aspx). 
It implements some good practices of the [layered architecture](http://msdn.microsoft.com/en-us/library/ms954595.aspx) and show you how create a specific model to generate application code. 

! Behaviors
* Multi views 
* Dependencies management using a public repository (not provided in this release) 
* Code generation uses strategies (You can create your own strategy) - Available strategies are nhibernate, wcf, static factory
* Dependency properties (specific to a strategy)
* Entities can be created directly from a database

! Screenshots

![Component Model](./img/samplecomponent.png)


[Entities model](./img/samplemodels.png)


[Dependencies graph on the repository](./img/dependenciesview.jpg)
