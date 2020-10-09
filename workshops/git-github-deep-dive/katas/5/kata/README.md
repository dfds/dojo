DFDS Git Training - Code Kata #5
======================================

This training exercise is a **beginner-level** course on Git version control and serves as a starting point for developers looking to onboard our [Github.com](https://github.com/dfds) efforts.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Git](https://git-scm.com/downloads)

## Exercise
In our fifth exercise we will take a close look at the `git commit` command and some of its uses.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata5
cd kata5
```

### 2. Initialize a new repository
Create a new repository to serve a sandbox for our kata:

```
git init playing-with-commit
```

### 3. Add some changes to our "main" branch
Once we have initialized `playing-with-commit` we can proceed to "stage" a file and commit it to our `main` branch:

```
cd playing-with-commit
echo HelloFile > text.txt
git commit -a -m "Foobar"
```

***Note*** <br/>
`git commit -a -m` is shorthand for `git add . & git commit -m`.

### 4. Undo changes in "last" commit and re-submit
Having just committed a "change set" to `playing-with-commit` we realize the we forgot to add a file and provide a proper commit message, luckily the `--amend` flag can help us with this. Lets try it out!:

```
echo HelloFile2 > text2.txt
git add text2.txt
git commit --amend -m "New commit message"
```

***Note*** <br/>
`git commit --amend` is more or less the same as doing `git reset --soft HEAD~1 & git commit`.

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).