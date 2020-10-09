#Clone the DFDS dojo repository
git clone https://github.com/dfds/dojo.git

#Verify remote with shortname "origin" has been added
git remote -v

#Add a new remote with a custom name
git remote add my-named-remote https://github.com/dfds/resource-provisioning-ssu-mvp

#Fetching our remote repository
git fetch my-named-remote

#Pushing to remote repository
git push my-named-remote master