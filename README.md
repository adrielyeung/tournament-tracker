# tournament-tracker
A Windows form application which allows users to start and track tournament rounds (organised in a single-elimination style).

## 1. Built With
- C# in Visual Studio
- Database managed by Microsoft SQL Management Studio
- SMTP Client to send out emails (Please set up a mail server, in ```App.config``` I have used an example GMail server. You will also need to enable "Less secure apps" for the server you set up, please see https://www.google.com/settings/security/lesssecureapps for example.)

## 2. Getting Started
1. Clone the repo
```
git clone https://https://github.com/adrielyeung/tournament-tracker
```

2. The solution and code can be run in Visual Studio. The ```Program.cs``` in TrackerUI is the main program to be run.

## 3. Usage
The dashboard will be displayed initially. Please select "Load Existing Tournaments" or "Create Tournament", then fill out the forms to create tournaments, teams and players.
![image](https://user-images.githubusercontent.com/43583274/112250044-bf26bc00-8c93-11eb-8432-2599c7422b62.png)

After creating the tournament and entering teams, the matchups will be randomly assigned by the system, and email alerts will be sent to all players. Please use the "Score" button to record the scores of each matchup.
![image](https://user-images.githubusercontent.com/43583274/112250234-fdbc7680-8c93-11eb-9c15-1fd4e8b36863.png)

After each team is progressed to next round, all players will receive email notifications. At the end of the tournament, an email announcement will be sent to all players in all teams.


## 4. Other features in the future
- Support other tournament systems, e.g. double-elimination, round-robin etc.
- Support more than 2 prize payouts (currently only champion and 1st runner up)
- More attractive UI, with diagrams to show the progress of each participating team
- Text alerts to users

## 5. Acknowledgements
This project could not be built without the tremendous help from Tim Corey's tutorial (https://www.iamtimcorey.com/).
