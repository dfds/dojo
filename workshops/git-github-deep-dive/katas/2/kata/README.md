DFDS Git Training - Code Kata #2
======================================

This training exercise is a **beginner-level** course on how to use Git for version control and serves as a starting point for developers looking to onboard Git at DFDS.


## Getting started

These instructions will help you prepare for the code kata and make sure that your local developer machine has the correct tools installed and configured.


### Prerequisites

* [Git](https://git-scm.com/downloads)
* [Visual Studio Code](https://code.visualstudio.com/download)


## Exercise

In our second exercise we will focus on substituting Gits default diff/merge tooling for Visual Studio Code in order to provide a GUI-based editor experience for conflict resolution, diffing, etc.


### 1. Create your kata directory
`mkdir kata2`<br/>
`cd kata2`


### 2. Configure git to use a new diff/merge tool
First we need to configure Git to use VS Code as our editor of choice. In order to register VS Code as the default diff/merge tool run the following commands:

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
Now that we have the tooling setup lets take a momement to verify that it works. We can do this by simply cloning the local repository we created in Kata #1 and changing a file via the following commands:

```
git clone ../kata1/my-local-repo your-local-repo
echo HelloWorld2 > your-local-repo/text.txt
```

### 5. Step into our repo and launch the git difftool
Once we have changed the file we can now as Git to launch our editor of choice to diff the contents in our index with that of the local repository:

```
cd your-local-repo
git difftool -y
```


## Want to help make our training material better?

 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/ded-dojo/issues).
