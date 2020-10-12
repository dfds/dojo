DFDS Git Training - Code Kata #6
======================================

This training exercise is a **beginner-level** course on Git version control and serves as a starting point for developers looking to onboard our [Github.com](https://github.com/dfds) efforts.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Git](https://git-scm.com/downloads)

## Exercise
In our sixth exercise we will take a close look at the `git rebase` command which has a reputation for being magical Git voodoo that beginners should stay away from. That's far from the truth & in this kata we will explore how it can actually make life much easier by making your git history tidy.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata6
cd kata6
```

### 2. Initialize a new repository
Create a new repository to serve a sandbox for our kata:

```
git init playing-with-rebase
```

### 3. Add some commits to our "master" branch
Once we have initialized `playing-with-rebase` we can proceed to add a few commits to built up a branch history so we can use it to try out the `git rebase` command:

```
cd playing-with-rebase

echo HelloFile > text.txt
git add text.txt
git commit -m "Commit #1 on main branch"

echo HelloFile2 > text2.txt
git add text2.txt
git commit -m "Commit #2 on main branch"
```

### 4. Create a new branch called "rebase-target" with some commits
In order to better understand how `git rebase` works we need to create a second branch with its own commit history:

```
git branch rebase-target
git checkout rebase-target

echo HelloFile3 > text3.txt
git add text3.txt
git commit -m "Commit #1 on rebase-target"
```

### 5. Checkout our "master" branch and add a new commit
By adding a new commit to `master` at this step we are forcing a divergence of the two branches:

```
git checkout master

echo HelloFile4 > text3.txt
git add text3.txt
git commit -m "Commit #3 on main"
```

### 6. Rebase "master" onto "rebase-target" 
Now that our branches have diverge we can re-consolidate the changes and tidy up our commit history via `git rebase`:

```
git checkout feature
git rebase master
```

### 7. Review "rebase-target" commit history
Once our changes are reconsolidated we can use `git log` to see the new commit history for `rebase-target`:

```
git log
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
