# bci-gaming

BCI Gaming Team is very cool

# Getting started

Install the required python packages for this project.

`pip install -r requirements.txt`

Download Unity for your device:

https://unity.com/download

To set up the game, start by cloning the repository by copying the repository URL and running 

`git clone <REPO URL>`

To run the game, open the unity hub and click "open" with the “add project from disk” option. 
Select the 'bci-game' folder in the 'bci-gaming' directory. 
Once unity opens the project, press play to ensure it runs correctly. 
Once you have made your changes to the game, stages the files to commit 

`git add <FILES TO ADD>` 

Then, commit your changes with a descriptive commit message

`git commit -m "<MESSAGE>"`

Connect to the remote repository 

`git remote add origin <REPO URL>`

Push your changes

`git push`

# Linting

Be sure to lint your code prior to opening pull requests! To lint python files, run:

`pylint ./bci-work`
