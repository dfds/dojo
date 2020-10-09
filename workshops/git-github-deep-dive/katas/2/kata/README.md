DFDS Git Training - Code Kata #2
======================================

This training exercise is a **beginner-level** course on Git version control and serves as a starting point for developers looking to onboard our [Github.com](https://github.com/dfds) efforts.

## Getting started
These instructions will help you prepare for the kata and ensure that your training machine has the tools installed you will need to complete the assignment(s). If you find yourself in a situation where one of more tools might not be available for your training environment please reach out to your instructor for assistance on how to proceed, post an [issue in our repository](https://github.com/dfds/dojo/issues) or fix it yourself and update the kata via a [pull request](https://github.com/dfds/dojo/pulls).


### Prerequisites
* [Git](https://git-scm.com/downloads)
* [Visual Studio Code](https://code.visualstudio.com/download)

## Exercise
In our second exercise we will focus on substituting Gits default diff/merge tooling for [Visual Studio Code](https://code.visualstudio.com/download) in order to provide a GUI-based editor experience for conflict resolution.

### 1. Create your kata directory
First we setup a directory for our exercise files. It's pretty straight forward:

```
mkdir kata2
cd kata2
```

### 2. Configure Git to use a new diff/merge tool
Then we need to configure Git to use [Visual Studio Code](https://code.visualstudio.com/download) as our editor of choice. In order to accomplish this we run the following commands:

```
git config --global merge.tool vscode
git config --global diff.tool vscode
```

### 3. Setup commands used by Git to launch Visual Studio code for diff/merge operations.
Having set our diff/merge tool defaults we can now proceed to configure the commands Git will use to launch our chosen editor:

```
git config --global mergetool.vscode.cmd "code --wait \"$MERGED\""
git config --global difftool.vscode.cmd "code --wait --diff \"$LOCAL\" \"$REMOTE\""
```

### 4. Clone a repo and make some changes so we can test that our difftool is working
Now that we have the tooling setup lets take a moment to verify that its working. We can do this by simply cloning the `my-local-repo` repository into a new repository named `your-local-repo` and staging a simple change:

```
git clone ../kata1/my-local-repo your-local-repo
echo HelloWorld2 > your-local-repo/text.txt
```

### 5. Step into our repo and launch the git difftool
Once we have changed the file we can instruct Git to launch our editor of choice and begin "diffing" the contents in our index to see how it has diverged from the "baseline" in `your-local-repo`:

```
cd your-local-repo
git difftool -y
```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).
