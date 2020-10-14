#Initialize a new Git repository
git init playing-with-commit

#Add some commits to our "master" branch
cd playing-with-rebase

echo HelloFile > text.txt
git add text.txt
git commit -m "Commit #1 on main branch"

echo HelloFile2 > text2.txt
git add text2.txt
git commit -m "Commit #2 on main branch"

#Create a new branch called "rebase-target" with some more commits
git branch rebase-target
git checkout rebase-target

echo HelloFile3 > text3.txt
git add text3.txt
git commit -m "Commit #1 on rebase-target"

#Verify last commit is the HEAD of our "rebase-target" branch
git log

#Checkout our "master" branch and add a new commit
git checkout master

echo HelloFile4 > text3.txt
git add text3.txt
git commit -m "Commit #3 on main"

#Verify last commit is the HEAD of our "master" branch
git log

#Rebase "master" onto "rebase-target" 
git checkout rebase-target
git rebase master

#Launch Git merge tool to fix merge conflicts
git mergetool -y

#Instruct Git to continue our rebase operation once conflicts are resolved
git rebase --continue

#Verify "rebase-target" commit history
git log