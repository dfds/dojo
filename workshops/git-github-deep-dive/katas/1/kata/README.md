DFDS Git Training - Code Kata #1
======================================

This training exercise is a **beginner-level** course on Git version control and serves as a starting point for developers looking to onboard our [Github.com](https://github.com/dfds) efforts.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Git](https://git-scm.com/downloads)

## Exercise
Our first exercise we will focus on how to initialize a Git repository on your local machine named `my-local-repo`. Once we have succeeded in this we add a simple text file to give you a better understanding of how easy it is to get started with Git.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata1
cd kata1
```

### 2. Create a new repository on your local machine
Next we have to instruct Git to initialize a new local repository called `my-local-repo`. It's pretty simple, run the following command:

```
git init my-local-repo
```

### 3. Create a file to be tracked by our local repository
Once the repository is initialized add a simple text file:

```
echo HelloWorld > my-local-repo/text.txt
```

### 4. Instruct Git to add the file to its "index"
Proceed to add a text file to our workspace and update our repository index to help Git keep track of the changes, this process is often referred to as ["staging"](https://git-scm.com/book/en/v2/Git-Basics-Recording-Changes-to-the-Repository). We achieve this by stepping into the root folder of our newly created `my-local-repo` repository and executing the `git add` command like so:

```
cd my-local-repo
git add text.txt
```

***Note*** <br/>
You can use `git add .` to perform bulk updates on the Git index.

### 5. Commit the file to the local repository
Once we have added the desired assets to our index we can then proceed to commit them to `my-local-repo` as follows:

```
git commit -m "Added text.txt"
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
