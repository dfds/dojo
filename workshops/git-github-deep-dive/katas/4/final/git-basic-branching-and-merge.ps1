#Initialize a new repository
git init playing-with-branches

#Add some changes to our "master" branch
cd playing-with-branches
echo HelloFile > text.txt
git add .
git commit -m "Adding a file to main branch"

#Create a new branch called  "our-new-branch"
git checkout -b our-new-branch

#Add some changes to our "our-new-branch"
echo HelloFile > text2.txt
git add .
git commit -m "Adding a file to our-new-branch branch"

#Verify that "our-new-branch" is ahead of "master"
git diff master our-new-branch

#Merge changes from "our-new-branch" to "master"
git checkout main
git merge our-new-branch

#Verify that "our-new-branch" is now aligned with "master"