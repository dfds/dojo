#Register VS Code as the default diff/merge tool
git config --global merge.tool vscode
git config --global diff.tool vscode

#Setup commands used by Git to launch Visual Studio code for diff/merge operations
git config --global mergetool.vscode.cmd "code --wait \"$MERGED\""
git config --global difftool.vscode.cmd "code --wait --diff \"$LOCAL\" \"$REMOTE\""

#Clone a repo and make some changes so we can test that our difftool is working
git clone ../../kata1/my-local-repo ../your-local-repo
echo HelloWorld2 > ../your-local-repo/text.txt

#Step into our repo and launch the git difftool
cd ../your-local-repo
git difftool -y