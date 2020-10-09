#Initialize a new repository
git init playing-with-branches

#Add some changes to our "main" branch
cd playing-with-branches
echo HelloFile > text.txt
git commit -a -m "Adding a file to main branch"

#Create a new branch called  "our-new-branch"
git checkout -b our-new-branch

#Add some changes to our "our-new-branch"
echo HelloFile > text2.txt
git commit -a -m "Adding a file to our-new-branch branch"

#Verify that "our-new-branch" has diverged from "main"
git status

#Merge changes from "our-new-branch" to "main"
git checkout main
git merge our-new-branch