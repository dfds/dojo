#Initialize a new repository
git init playing-with-files

#Stage a file for commit in our newly created repository
cd playing-with-files
echo HelloFile > text.txt
git add .

#Commit the tracked changes
git commit -m "Add existing file"

#Rollback our commit to undo changes
git update-ref -d HEAD

#Unstage our changes from the index
git rm text.txt --cached