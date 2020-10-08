#Initialize a new repository
git init playing-with-files

#Stage a file for commit in our newly created repository
cd playing-with-files
echo HelloFile > text.txt
git add .

#Commit the tracked changes
git commit -m "Add existing file"

#Remove the commit we just created to modify files
git reset --soft HEAD~1

#Unstage our file from the index
git reset HEAD text.txt