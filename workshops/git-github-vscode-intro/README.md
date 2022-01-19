---
marp: true
_class: lead
paginate: true
headingDivider: 2
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

- Prerequisites/assumptions
  - Windows 10/11
  - Local admin rights
  - Dev GPO's
- Brief markdown
  - https://commonmark.org/help/
- Clone repo after setting up SSH
- Re-write kata 1 and include (remember elevated vs. user context)
- Mention frequent, small commits
- Update agenda

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

---

## Git

![bg left:40% 80%](../../assets/git-logo.svg)

- Source Code Management (SCM) tool
- Free and open-source
- Created by Linus Torvalds in 2005 for development of the Linux kernel

---

### Git concepts (1/2)

- Distributed Version Control System
- *Can* work 100% local
  - You can enable version control in any folder
  - No external server or service required
  - But you probably want one
- Tracks changes to files

---

### Git concepts (2/2)

- Bundle changed files into a commit
  - Commit changes, or...
  - ...revert to previous commit

---

### Exercise: Install Git

- Install via *Software Center*, or
- Download and install from <https://git-scm.com/download/>

---

### Basic Git commands

| Command           | Effect                                                     |
| ----------------- | ---------------------------------------------------------- |
| `init`            | Initialise a local Git repo in the current directory       |
| `clone <repo>`    | Createa a local clone of the repo at the specified URL     |
| `pull`            | Pull any remote changes from the remote repo (or *origin*) |
| `add <file>`      | *Stage* specified, or all, files for a new *commit*        |
| `commit -m <msg>` | Commit staged files and include the specified message      |
| `push`            | Push local commits to *origin*                             |
| `status`          | Display status of staged files, and local vs. remote repo  |

---

### Exercise: Get started with local Git repo

- Create a common directory to store code, e.g.:
  - `C:\code`
  - `~/code`

Then follow the kata at
<https://github.com/dfds/dojo/tree/master/workshops/git-github-deep-dive/katas/1/kata>

---

## GitHub

![bg left:40% 80%](../../assets/github-logo.svg)

- Free for all, paid tiers available

---

### Demo: Walk-through of main GitHub features

- Git-as-a-Service
- Issue tracking
- Kanban boards
- GitHub Actions

---

## Visual Studio Code

![bg left:40% 50%](../../assets/vscode-logo.svg)

Extendable, free, open-source code editor

---

### Demo: Getting started with VS Code

- Overview
- Keyboard shortcuts
  - `Ctrl`+`Shift`+`P` (or simply `F1` apparently :exploding_head:)
    - Show the (powerful!) command palette
  - See <https://code.visualstudio.com/docs/getstarted/keybindings> for more info and cheat sheet

---

### Demo: VS Code Git integration

---

### VS Code extensions

- VS Code is extendable via a wide range of extensions
- Features include:
  - VS Code UI themes
  - Auto-formatting code
  - Linting and security scanning
- Out-of-scope for this workshop

---

### Exercise: Installing VS Code

- Download and install from <https://code.visualstudio.com/> (local admin rights not required)

*Do not recommend installing via Software Center (per-machine).*

---

### Demo: Cool VS Code features

Short introduction to useful VS Code features, you can explore further on your own:

- Remote VS code - target WSL, remote computers, Docker containers
  - See [Microsoft: VS Code Remote Development](https://code.visualstudio.com/docs/remote/remote-overview)
- VS Code is built into GitHub - press `.` when browsing a repo

## Tying it all together

### Walk-through exercise: Clone a public GitHub repo locally

<https://github.com/dfds/cloudwatchlogs-collector>

---

### Configuring Git

- Git configuration has multiple scopes:
  - `system`: Machine-wide (`$GIT_BIN_DIR/etc/gitconfig`)
  - `global`: Current user, all repos (`~/.gitconfig`)
  - `local`: Current repo only (`.git/config`)
- The more specific scope, the higer precedence
  - `local` overrides `global`, which overrides `system`

---

### Exercise: Examine and adjust Git configuration

```powershell
# List current configuration and scope
git config --list --show-scope

# Configure minimal user info and disable "autocrlf"
git config --global user.name "Jane Doe"
git config --global user.email jadoe@dfds.com
git config --global core.autocrlf=false
```

---

### SSH key authentication and GitHub

- SSH keys are assymetrical - they have a public and private/sensitive part
- You can add the *public* part of your SSH keys to your GitHub account
- With the private part of those SSH keys, you can authenticate against your GitHub account

---

### Walk-through exercise: Setup SSH key authentication for GitHub (1/2)

- Add the SSH client capability to Windows
  - `Get-WindowsCapability -Online -Name OpenSSH.Client* | Add-WindowsCapability -Online`
- Generate SSH key, enable `ssh-agent` and add key to agent
  - <https://docs.microsoft.com/en-us/windows-server/administration/openssh/openssh_keymanagement#user-key-generation>
- Configure SSH config to use key file for GitHub and forward agent (LF!):

```text
Host github.com
        ForwardAgent yes
```

---

### Walk-through exercise: Setup SSH key authentication for GitHub (2/2)

- Configure Git to use Windows' OpenSSH binary and config
  - `git config --global core.sshCommand "'$((Get-Command ssh).Source)' -T"`
- Add key to GitHub profile
  - `Get-Content .\id_ed25519.pub | Set-Clipboard`
- Verify GitHub authentication
  - `ssh -T git@github.com`
  - `Hi <GitHubUsername>! You've successfully authenticated, but GitHub does not provide shell access.`

---

### Exercise: Track a local

- Clone
- Make changes
- Git status
- Setting as upstream to local repo

```
git remote add origin git@github.com:abstrask/github-workshop.git
git branch -M main
git push -u origin main
```

- Clone
- Make changes
- Git status

---

## Overview of Git branches

![bg left:40% 50%](https://upload.wikimedia.org/wikipedia/commons/e/ed/Octicons-git-branch.svg)

---

- Why branches?
- Concepts
  - PRs
  - PR reviews
  - Merge
  - Branch protection
