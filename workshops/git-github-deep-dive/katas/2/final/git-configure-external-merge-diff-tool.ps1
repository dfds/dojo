#Configure Git to use a new diff/merge tool
git config --global merge.tool vscode
git config --global diff.tool vscode

#Setup commands used by Git to launch Visual Studio code for diff/merge operations
git config --global mergetool.vscode.cmd "code --wait \"$MERGED\""
git config --global difftool.vscode.cmd "code --wait --diff \"$LOCAL\" \"$REMOTE\""

#Clone "my-local-repo" into "your-local-repo" and make some changes
git clone ../../kata1/my-local-repo ../your-local-repo
echo HelloWorld2 > ../your-local-repo/text.txt

#Step into "your-local-repo" and perform diff operations
cd ../your-local-repo
git difftool -y

#Verify that no files need merging
git mergetool