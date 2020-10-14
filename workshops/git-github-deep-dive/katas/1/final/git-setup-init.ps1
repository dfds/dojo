#Create a new git repository
git init my-local-repo

#Create a file to be tracked by "my-local-repo"
echo HelloWorld > my-local-repo\text.txt

#Instruct Git to "stage" our file
git add my-local-repo\text.txt

#Commit staged changes to "my-local-repo"
git commit -m "Added text.txt"

#Verify that our commit was successful
git log