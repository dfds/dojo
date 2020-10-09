DFDS Git Training - Code Kata #7
======================================

This training exercise is a **beginner-level** course on Git version control and serves as a starting point for developers looking to onboard our [Github.com](https://github.com/dfds) efforts.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).

### Prerequisites
* [Git](https://git-scm.com/downloads)

## Exercise
In our last exercise we will take a closer look at the `git remote` concept and how we can connect our local repository to remote repositories.

### 1. Create a kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata7
cd kata7
```

### 2. Clone the DFDS dojo repository
It's simple, just use the following command:

```
git clone https://github.com/dfds/dojo.git
```

### 3. Verify remote with shortname "origin" has been added
Once we clone the dojo we can try out the `git remote` command to verify that a remote with the shortname `origin` has been added. The name `origin` is implicitly assigned by `git clone`:

```
git remote -v
```

***Note*** <br/>
When you use the -v flag it shows you the URLs that Git has stored for the shortname to be used when reading and writing to that remote.

### 4. Add a new remote with a custom name
In some cases we want to add a remote without cloning it or we simply want to control the shortname assigned to a given remote repository. In order to do this we can use the `git remote` command directly to avoid cloning the contents of the remote repository until we are need it:

```
git remote add my-named-remote https://github.com/dfds/resource-provisioning-ssu-mvp
```

### 5. Fetching our remote repository
Once we have added `my-named-remote` we can proceed to fetching the contents of the remote repository. In order to do this we simply run the following command:

```
git fetch my-named-remote
```

### 6. Pushing to remote repository
After a while we might want to push changes in our local repository back to the remote. To do this we use the `git push <remote> <branch>` command as follows:

```
git push my-named-remote master
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).