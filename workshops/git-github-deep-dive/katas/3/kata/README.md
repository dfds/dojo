DFDS Git Training - Code Kata #3
======================================

This training exercise is a **beginner-level** course on how to use Git for version control and serves as a starting point for developers looking to onboard Git at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Git](https://git-scm.com/downloads)


## Exercise

In our third exercise we will learn how to use the Git command line to add and remove files in a repository.


### 1. Create a kata directory
`mkdir kata3`<br/>
`cd kata3`


### 2. Initialize a new repository
Create a new repository to serve a sandbox for our kata.

```
git init playing-with-files
```

### 3. Stage a file for commit in our newly created repository
Create a file that we can manipulate via Git to add/remove it from our local repository.

```
cd playing-with-files
echo HelloFile > text.txt
git add .
```

### 4. Commit the tracked changes
Commit changes to our local repository.

```
git commit -m "Add existing file"
```

### 5. Remove the commit we just created to modify files
Rollback the last commit we applied to our local repository in order to remove our file.

```
git reset --soft HEAD~1
```

### 6. Unstage our file from the index
Unstage our file from the index so we dont add it to the local repository in future commits.
 
```
git reset HEAD text.txt
```


## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).