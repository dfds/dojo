#Clone and enter the DFDS dojo
git clone https://github.com/dfds/dojo.git
cd dojo

#Verify remote with shortname "origin" has been added
git remote -v

#Add a new remote with a custom name
git init ../my-named-remote-repo
cd ../my-named-remote-repo
git remote add my-named-remote https://github.com/dfds/resource-provisioning-ssu-mvp

#Fetching our remote repository
git fetch my-named-remote

#Pull from remote
git pull my-named-remote master

#Push to remote
git push --set-upstream my-named-remote master