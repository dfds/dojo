#Initialize a new repository
git init playing-with-commit

#Add some changes to our "main" branch
cd playing-with-commit
echo HelloFile > text.txt
git commit -a -m "Foobar"

#Create a new branch called  "our-new-branch"
echo HelloFile2 > text2.txt
git add text2.txt
git commit --amend -m "New commit message"