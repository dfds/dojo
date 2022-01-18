---
marp: true
_class: lead
paginate: true
backgroundColor: #fff
backgroundImage: url('https://marp.app/assets/hero-background.svg')
---

# Git, GitHub and VS Code 101

Rasmus Rask
January 2022

---

## Agenda (1/2)

- Git
  - Git concepts
  - Installing Git
  - Configuring Git
  - Basic Git commands
- GitHub
  - Getting started with GitHub
  - GitHub feature walk-through

---

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

![bg left:40% 80%](https://upload.wikimedia.org/wikipedia/commons/e/e0/Git-logo.svg)

- Open-source, distributed version control system (VCS)
- Created by Linus Torvalds in 2005 for development of the Linux kernel

---

### Git concepts

- Distributed Version Control System
- Everything is in .git folder
  - You can enable version control in any folder
  - No external server or service required
  - But you probably want one
- File tracking, `.gitignore`
- Repos, commits
- Git client configuration and scopes

---

### Installing Git

- Software Center or <https://git-scm.com/download/>

---

### Configuring Git

- LF
- User, mail
- ?

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

### Git exercises

1. Install and configure global settings
2. Create local repo

---

## GitHub

![bg left:40% 80%](https://upload.wikimedia.org/wikipedia/commons/2/29/GitHub_logo_2013.svg)

- Free for all, paid tiers available
- Git-as-a-Service
- Issue tracking
- Kanban boards
- GitHub Actions

---

### Getting started

---

### GitHub repos: Git-as-a-Service

- Browsing code
- Built-in VS Code - just press `.`
- Authenticate using SSH key
- Security scanning

---

### GitHub issues

<https://github.com/features/issues/>

---

### GitHub Actions

---

### Github exercises

- Setting as upstream to local repo

---

## Visual Studio Code

![bg left:40% 50%](https://upload.wikimedia.org/wikipedia/commons/9/9a/Visual_Studio_Code_1.35_icon.svg)

- Free, open-source

---

### Getting started

- <https://code.visualstudio.com/>

*Do not recommend installing via Software Center (per-machine).*

---

## Working with branches

![bg left:40% 50%](https://upload.wikimedia.org/wikipedia/commons/e/ed/Octicons-git-branch.svg)

---

- Why branches?
- Concepts
  - PRs
  - PR reviews
  - Merge
  - Branch protection
