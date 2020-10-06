#Create a local repo
git init my-local-repo

#Create a file to be tracked by our repo
echo HelloWorld > my-local-repo\text.txt

#Instruct Git to add the file to its "index"
git add my-local-repo\text.txt

#Commit the file to the local repository
git commit -m "Added text.txt"
