DFDS Git Training - Code Kata #1
======================================

This training exercise is a **beginner-level** course on how to use Git for version control and serves as a starting point for developers looking to onboard Git at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Git](https://git-scm.com/downloads)


## Exercise

In our first exercise we will focus on setting up a local repository and subsequently cloning it to give us a better understanding of how version control works in a peer-2-peer context (every client is a server).


### 1. Create your project directory
`mkdir kata1`<br/>
`cd kata1`


### 2. Create a new repository on your local machine
It's pretty simple, navigate to desired root directory (/kata1) and run the following command:

```
git init my-local-repo
```


### 3. Create a file to be tracked by our local repository
Once the repository is initialized run the following command to create a simple text file:

```
echo HelloWorld > my-local-repo/text.txt
```


### 4. Instruct Git to add the file to its "index"
Now that we have added an asset to our workspace we need to update our index to help Git keep track of our changes. This is achieved by stepping into the root folder of our newly created repository and running the "add" command:

```
cd my-local-repo
git add text.txt
```

***Note*** <br/>
You can use `git add .` to perform bulk updates on the Git index. We will visit `git add` in more detail in later katas.


### 5. Commit the file to the local repository
Once we have added the desired assets to our index we can the proceed to commit them to the local repository via the following command:

```
git commit -m "Added text.txt"
```


## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
