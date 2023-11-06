# Project _NAME_

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: _Ricky Yu_
-   Section: _02_

## Simulation Design

_In a huge "room" (The whole screen) filled with people/cells wandering around. Able to put down an "infected" person/cell or "infect" one.
   The infection may spread. There will be "healers" around to fight it.[^1]_

### Controls

-   _Drop in an "infect" person/cell_
    -   _Mouse input_
    -   _The infected will chase uninfected, slowly converting all the uninfected if untreated_

-   _UI input (speed mode)[^2]_
    -   _Ui input_
    -   _Make the simulation run faster (**maybe**)_

-   _UI input (restart)[^3]_
    -   _Ui input_
    -   _restart the simulation_

## _Agent 1 (Uninfected)_

_The main object on the screen. Will "wander" aimlessly. Will run from infected._

### _State 1 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
   - _If behavior has input data list it here_
   - _eg, Flee - nearest Agent2_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   - _eg, When this agent gets within range of Agent2_
   - _eg, When this agent has reached target of State2_
   
### _State 2 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## _Agent 2 (Infected)_

_The enemy of the simulation. Will chase afterward nearest (if possible) uninfected. May be defeated by "healers"_

### _State 1 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   
### _State 2 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_

## Make it Your Own

- _List out what you added to your game to make it different for you_
- _If you will add more agents or states make sure to list here and add it to the documention above_
- _If you will add your own assets make sure to list it here and add it to the Sources section

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

