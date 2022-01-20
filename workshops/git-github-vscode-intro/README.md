---
marp: true
_class: lead
paginate: true
headingDivider: 3
backgroundColor: #fff
backgroundImage: url('../../assets/hero-background.svg')
header: '**Git, GitHub and VS Code 101**'
footer: 'DFDS Cloud Engineering, January 2022'
duration: 180min
---

# Git, GitHub and VS Code 101

DFDS Cloud Engineering
Updated January 2022

## To do

- First
  - Clone repo after setting up SSH
- After
  - Prerequisites/assumptions
    - Windows 10/11
    - Local admin rights
    - Dev GPO's
  - Update agenda
  - Re-write kata 1 and include (remember elevated vs. user context)

## Agenda (1/2)

- Git
  - Git concepts
  - Configuring Git
  - Basic Git commands
- GitHub
  - Getting started with GitHub
  - Walk-through of main GitHub features

## Agenda (2/2)

- VS Code
  - Installing VS Code
  - Introduction to VS Code
  - Plugins in VS Code
  - Git integration in VS Code
- Working with Git branches
  - Why branches?
  - Branch concepts

## Git

![bg left:40% 80%](../../assets/git-logo.svg)

- Source Code Management (SCM) tool
- Free and open-source
- Created by Linus Torvalds in 2005 for development of the Linux kernel

### Git concepts (1/2)

- Distributed Version Control System
- *Can* work 100% local
  - You can enable version control in any folder
  - No external server or service required
  - But you probably want one
- Tracks changes to files

### Git concepts (2/2)

- Bundle changed files into a commit
  - Commit changes, or...
  - ...revert to previous commit
- Commits should be atomic and frequent

### Exercise: Install Git

- Install via *Software Center*, or
- Download and install from <https://git-scm.com/download/>

### Basic Git commands

| Command           | Effect                                                     |
| ----------------- | ---------------------------------------------------------- |
| `init -b main`    | Initialise a local Git repo in the current directory       |
| `clone <repo>`    | Createa a local clone of the repo at the specified URL     |
| `pull`            | Pull any remote changes from the remote repo (or *origin*) |
| `add <file>`      | *Stage* specified, or all, files for a new *commit*        |
| `commit -m <msg>` | Commit staged files and include the specified message      |
| `push`            | Push local commits to *origin*                             |
| `status`          | Display status of staged files, and local vs. remote repo  |

### Exercise: Get started with local Git repo

- Create a common directory to store code, e.g.:
  - `C:\code`
  - `~/code`

Then follow the kata at
<https://github.com/dfds/dojo/tree/master/workshops/git-github-deep-dive/katas/1/kata>

## GitHub

![bg left:40% 80%](../../assets/github-logo.svg)

- Free for all, paid tiers available

### Demo: Walk-through of main GitHub features

- Git-as-a-Service
- Issue tracking
- Kanban boards
- GitHub Actions

## Visual Studio Code

![bg left:40% 50%](../../assets/vscode-logo.svg)

Extendable, free, open-source code editor

### Demo: Getting started with VS Code

- Overview
- Keyboard shortcuts :keyboard:
  - `Ctrl`+`Shift`+`P` (or simply `F1` apparently :exploding_head:)
    - Show the (powerful!) command palette
    - Displays assigned shortcuts (learn and use them)
  - Cheat sheet and more info, see <https://code.visualstudio.com/docs/getstarted/keybindings>.
- Git integration

### VS Code extensions

- VS Code is extendable via a wide range of extensions
- Features include:
  - VS Code UI themes
  - Auto-formatting code
  - Linting and security scanning
- Out-of-scope for this workshop

### Exercise: Installing VS Code

- Download and install from <https://code.visualstudio.com/>
- Local admin rights not required
- Built-in auto-update

*Do not recommend installing via Software Center as this installs "per-machine",<br>and has a slower update cycle.*

### Demo: Cool VS Code features

Short introduction to useful VS Code features, you can explore further on your own:

- Remote VS code - target WSL, remote computers, Docker containers
  - See [Microsoft: VS Code Remote Development](https://code.visualstudio.com/docs/remote/remote-overview)
- VS Code is built into GitHub - press `.` when browsing a repo

## Tying it together

![bg left:40% 100%](../../assets/knot.svg)

### Configuring Git

- Some settings need to be configured to work with remote repositories
- Git configuration has multiple scopes:
  - `system`: Machine-wide (`$GIT_BIN_DIR/etc/gitconfig`)
  - `global`: Current user, all repos (`~/.gitconfig`)
  - `local`: Current repo only (`.git/config`)
- The more specific scope, the higer precedence
  - `local` overrides `global`, which overrides `system`

### Exercise: Examine and adjust Git configuration

- Examine the current Git configuration settings and their scope
- Configure name and email address
- Disable automatic End-of-Line character conversion

```powershell
# List current configuration and scope
git config --list --show-scope

# Configure minimal user info and disable "autocrlf"
git config --global user.name "Jane Doe"
git config --global user.email jadoe@dfds.com
git config --global core.autocrlf=false
```

### SSH key authentication and GitHub

- SSH keys are assymetrical - they have a public and private/sensitive part
- You can add the *public* part of your SSH keys to your GitHub account
- With the private part of those SSH keys, you can authenticate against your GitHub account

### Walk-through exercise: Setup SSH key authentication for GitHub (1)

- Add the SSH client capability to Windows
  - `Get-WindowsCapability -Online -Name OpenSSH.Client* | Add-WindowsCapability -Online`
- Generate SSH key
  - `ssh-keygen -t ed25519`
- Enable `ssh-agent` (*must run as administrator/elevated*):

```powershell
Get-Service ssh-agent | Set-Service -StartupType Manual
Start-Service ssh-agent
```

### Walk-through exercise: Setup SSH key authentication for GitHub (2)

- Add key to agent
  - `ssh-add ~\.ssh\id_ed25519`
- Configure SSH config to use key file for GitHub and forward agent (LF!):

```text
Host github.com
        ForwardAgent yes
```

### Walk-through exercise: Setup SSH key authentication for GitHub (3)

- Configure Git to use Windows' OpenSSH binary and config
  - `git config --global core.sshCommand "'$((Get-Command ssh).Source)' -T"`
- Copy public key to clipboard and add to GitHub profile
  - `Get-Content .\id_ed25519.pub | Set-Clipboard`
  - <https://github.com/settings/keys>
- Verify GitHub authentication
  - `ssh -T git@github.com`

Expected output:

```text
Hi <GitHubUsername>! You've successfully authenticated, but GitHub does not provide shell access.
```

### Exercise: Clone a public GitHub repo locally (1)

![bg right:25% 100%](./assets/github-code-clone.png)

- Browse to any public GitHub repository
  - E.g. <https://github.com/dfds/cloudwatchlogs-collector>
- Click the green `Code` button
- Select the "SSH" tab
- Click the copy button next to the `git@github...` string
- Open a terminal and go your root code directory
- Type `git clone `, paste in the string from the clipboard and run the command
- Verify you have a sub-directory with the name and contents of the cloned repo

### Exercise: Clone a public GitHub repo locally (2)

- Open the repository you just cloned in VS Code
  - `code <RepoName>` (or `code .` to open current directory)
- Make a few file changes
  - Create a new, modify and existing, delete another
- Examine how the changes are represented in VS Code
- In the terminal change into the repo directory, and run `git status`
- Delete the directory with the cloned repo

### Exercise: Start tracking a local repo

- Create a new, local repo, and add a text file

```powershell
cd (New-Item -Name github-workshop -Type Directory)
git init -b main
"Hello" | Out-File -Path hi.txt
```

- Create a new repository in GitHub, e.g. "github-workshop"
  - <https://github.com/new>
- In the terminal, configure the new GitHub repo as the *origin* and push your local repo to it

```powershell
git remote add origin git@github.com:<GithubUsername>/<RepoName>.git
git branch -M main
git push -u origin main
```

## Overview of Git branches

![bg left:40% 50%](https://upload.wikimedia.org/wikipedia/commons/e/ed/Octicons-git-branch.svg)

- Why branches?
- Concepts
  - PRs
  - PR reviews
  - Merge
  - Branch protection

## Where to go from here

- Brief markdown
  - https://commonmark.org/help/
  - This slidedeck
- Create repos in GH
- Project boards
- Modify issues, move around
