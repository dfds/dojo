DFDS Git Training - Code Kata #3
======================================

This training exercise is a **beginner-level** course on Git version control and serves as a starting point for developers looking to onboard our [Github.com](https://github.com/dfds) efforts.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Git](https://git-scm.com/downloads)


## Exercise
In our third exercise we will learn how to use the Git command line to add and remove files in a repository.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata3
cd kata3
```

### 2. Initialize a new Git repository
Then we initialize a new repository named `playing-with-files` to serve a sandbox for our kata:

```
git init playing-with-files
```

### 3. Stage a file for commit in our newly created repository
Once we have initialized `playing-with-files` we can proceed to "stage" a file:

```
cd playing-with-files
echo HelloFile > text.txt
git add .
```

### 4. Commit the tracked changes
When then changes have been "staged" by Git we can commit them to our `playing-with-files` repository and use the `-m` flag to specify a commit message:

```
git commit -m "Add existing file"
```

### 5. Rollback our commit to undo changes
Rollback the initial commit we applied to `playing-with-files` in order to undo changes:

```
git update-ref -d HEAD
```

***Note*** <br/>
The above command we are using is ONLY for the initial commit (also called the root commit). If you are working on a branch with an existing commit history the correct way to achieve this would be via `git reset --soft HEAD~1`. However in newly created repositories this command will not work thus we are forced to use `git update-ref -d HEAD` which essentially deletes the branch and recreates it.

### 6. Unstage our changes from the index
Once we have succesfully reset the "tip" of our `playing-with-files` repository we can safely remove file(s) from the index so we don't add them in future commits:
 
```
git rm text.txt --cached
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
